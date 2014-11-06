using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using BL.Calculator;

namespace BL.Caching
{
    public static class CacheHelper
    {
        private static readonly Dictionary<string, string> KeysGroups = new Dictionary<string, string>();

        public static readonly string AnonymousGroup = string.Empty;

        /// <summary>
        /// Não foi usado Auto-Properties pois a valor inicial é true. Para isso seria necessário alterar o valor no construtor static. Construtores static degradam a performance.
        /// </summary>
        private static bool _cacheEnabled = true;
        public static bool CacheEnabled { get { return _cacheEnabled; } set { _cacheEnabled = value; } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Não há outra técnica para isto. E não aumenta a complexidade já que a expression é um syntactic sugar, o cliente apenas escreve um lambda")]
        public static TResult FromCacheOrExecute<TResult>(this Cache cache, Expression<Func<TResult>> func)
        {
            return FromCacheOrExecute(cache, func, null, null);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Não há outra técnica para isto. E não aumenta a complexidade já que a expression é um syntactic sugar, o cliente apenas escreve um lambda")]
        public static TResult FromCacheOrExecute<TResult>(this Cache cache, Expression<Func<TResult>> func, string key)
        {
            return FromCacheOrExecute(cache, func, key, null);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Não há outra técnica para isto. E não aumenta a complexidade já que a expression é um syntactic sugar, o cliente apenas escreve um lambda")]
        public static TResult FromCacheOrExecute<TResult>(this Cache cache, Expression<Func<TResult>> func, CacheOptions cacheOptions)
        {
            return FromCacheOrExecute(cache, func, null, cacheOptions);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "MethodCallExpression"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Não há outra técnica para isto. E não aumenta a complexidade já que a expression é um syntactic sugar, o cliente apenas escreve um lambda")]
        public static TResult FromCacheOrExecute<TResult>(this Cache cache, Expression<Func<TResult>> func, string key, CacheOptions cacheOptions)
        {
            if (cache == null)
                throw new ArgumentNullException("cache");

            if (func == null)
                throw new ArgumentNullException("func");

            cacheOptions = cacheOptions ?? new CacheOptions();

            if (string.IsNullOrEmpty(key))
            {
                MethodCallExpression method = func.Body as MethodCallExpression;

                if (method == null)
                {
                    throw new InvalidCachedFuncException("Body must be MethodCallExpression to auto generate a cache key");
                }

                key = "";// CacheKeyGenerator.GenerateKey(method);
            }

            object returnObject = null;

            if (CacheEnabled)
            {
                returnObject = cache[key];
            }

            if (returnObject is TResult)
            {
                //Garante que o nome do Grupo será atualizado
                AddKeyOnGroup(cacheOptions.GroupName, key);
            }
            else
            {
                Action initializer = cacheOptions.ExecutionInitializer;
                if (initializer != null)
                {
                    initializer();
                }
                returnObject = func.Compile()();
                if (returnObject != null)
                {
                    cache.Add(key, returnObject, cacheOptions.Dependencies, cacheOptions.AbsoluteExpiration,
                              cacheOptions.SlidingExpiration, cacheOptions.Priority, CacheItemRemovedCallback);
                    AddKeyOnGroup(cacheOptions.GroupName, key);
                }
            }
            return (TResult)returnObject;
        }

        #region Groups

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "O método pode demorar caso tenha muitas chaves, além de poder gerar resultados diferentes em duas chamadas consecutivas.")]
        public static ILookup<string, string> GetAllKeys()
        {
            lock (KeysGroups)
            {
                return KeysGroups.ToLookup(x => x.Value, x => x.Key);
            }
        }

        public static IEnumerable<string> GetKeysByGroup(string groupName)
        {
            lock (KeysGroups)
            {
                return KeysGroups.Where(x => x.Value == groupName).Select(x => x.Key).ToList();
            }
        }

        public static IEnumerable<string> Groups
        {
            get
            {
                lock (KeysGroups)
                {
                    return KeysGroups.Values.Distinct().ToList();
                }
            }
        }

        public static void RemoveCacheByGroup(string groupName)
        {
            IEnumerable<string> chaves = GetKeysByGroup(groupName);
            foreach (string chave in chaves)
            {
                HttpRuntime.Cache.Remove(chave);
            }
        }

        public static void ClearCache()
        {
            foreach (DictionaryEntry cache in HttpRuntime.Cache)
            {
                HttpRuntime.Cache.Remove(cache.Key.ToString());
            }
        }

        private static void AddKeyOnGroup(string nomeGrupo, string chave)
        {
            lock (KeysGroups)
            {
                KeysGroups[chave] = nomeGrupo;
            }
        }

        private static void CacheItemRemovedCallback(string chave, object value, CacheItemRemovedReason reason)
        {
            lock (KeysGroups)
            {
                KeysGroups.Remove(chave);
            }
        }

        #endregion
    }
}
