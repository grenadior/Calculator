using System;
using System.Collections.Generic;
using System.Linq;
using BO;
using BO.Implementation;
using BO.Implementation.Caching;
using CalculatorZd.Models;
using Common.Api;

namespace CalculatorZd.Services
{
    public class FilterService
    {
        public static IEnumerable<string> GetFilterListByTerm(int termLength, string term, string filterName,
            int lengthListShow,
            string defaultValue = "")
        {
            var items = DictionaryHelper.GetDictionaryByFilter<List<string>>(filterName);

            if (!String.IsNullOrEmpty(term) && term.Length > 0 && term.Length >= termLength)
            {
                return GetFilteredObjectList(term, items, defaultValue, lengthListShow);
            }
            return null;
        }


        public static IEnumerable<string> GetFilterListByTerm(int termLength, string term, string filterNameFirst,
            string filterNameSecond,
            int lengthListShow,
            string defaultValue = "", string cacheKey = "")
        {
            var items = DictionaryHelper.GetDictionaryByFilter<List<string>>(filterNameFirst, filterNameSecond, cacheKey);

            if (!String.IsNullOrEmpty(term) && term.Length > 0 && term.Length >= termLength)
            {
                return GetFilteredObjectList(term, items, defaultValue, lengthListShow);
            }
            return null;
        }

        public static IEnumerable<string> GetFilterList(string filterName, string defaultValue, string orderBy = "")
        {
            var items = DictionaryHelper.GetDictionaryByFilter<List<string>>(filterName);
            return GetObjectList(items, defaultValue);
        }
        
        private static IEnumerable<string> GetObjectList(List<string> items, string defaultValue)
        {
            if (!String.IsNullOrEmpty(defaultValue))
                items.Insert(0, defaultValue);

            var types = new List<FilterTypeViewModel>();

            foreach (string item in items)
            {
                if (item != null && !String.IsNullOrEmpty(item.Trim()))
                {
                    types.Add(new FilterTypeViewModel
                    {
                        name = item
                    });
                }
            }
            return items;
        }

        public static List<FilterWagonTypeViewModel> GetWagonTypes()
        {
            var model = new List<FilterWagonTypeViewModel>();
            List<WagonGroupType> wagonGroupTypes = FilterManager.GetWagonTypesHierarhy();

            model.Add(new FilterWagonTypeViewModel {ID = -1, WagonGroup = null, WagonName = Constants.SELECT_VALUES});

            foreach (WagonGroupType groupItemType in wagonGroupTypes)
            {
                model.Add(new FilterWagonTypeViewModel
                {
                    ID = groupItemType.WagonGroupID,
                    WagonName = groupItemType.WagonGroupName
                    // WagonGroup = groupItemType.WagonGroupName
                });
                foreach (WagonTypes wagonItemType in groupItemType.WagonTypes)
                {
                    model.Add(new FilterWagonTypeViewModel
                    {
                        ID = wagonItemType.WagonTypeID,
                        WagonName =  "---" + wagonItemType.WagonTypeName,
                       // WagonGroup = groupItemType.WagonGroupName
                    });
                }
            }

            return model;
        }


        private static IEnumerable<string> GetFilteredObjectList(string term, List<string> items, string defaultValue,
            int lengthListShow)
        {
            if (!String.IsNullOrEmpty(defaultValue))
                items.Add(defaultValue);

            var filteredItems = new List<string>();
            foreach (string item in items)
            {
                if (item != null && item.ToLower().IndexOf(term.ToLower(), StringComparison.Ordinal) >= 0)
                    filteredItems.Add(item);
            }


            IEnumerable<string> itemsResult = filteredItems.Take(lengthListShow);


            return itemsResult;
        }

        public static IEnumerable<FilterTypeViewModel> GetFilterListAll(string filterName, string firstStringValue = "",
            string orderBy = "")
        {
            var items = DictionaryHelper.GetDictionaryByFilter<List<string>>(filterName);
                // CalculatorManager.GetEntities(filterName, "", "", orderBy);
            if (items == null)
                return null;

            var types = new List<FilterTypeViewModel>();

            if (!String.IsNullOrEmpty(firstStringValue))
                types.Add(new FilterTypeViewModel
                {
                    name = firstStringValue
                });

            foreach (string item in items)
            {
                if (item == null ||
                    (!String.IsNullOrEmpty(firstStringValue) && item.Equals(Constants.DefaultCountryName)))
                    continue;

                if (!String.IsNullOrEmpty(item.Trim()))
                {
                    types.Add(new FilterTypeViewModel
                    {
                        name = item
                    });
                }
                else
                {
                    types.Add(new FilterTypeViewModel
                    {
                        name = item
                    });
                }
            }

            return types;
        }
    }
}