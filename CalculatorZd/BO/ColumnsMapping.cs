using System;

namespace BO
{
    public enum ColumnsMapping
    {
        [StringValue("[Номер вагона]")] NumberVagon = 1,
        [StringValue("[Номер контейнера]")] NumberContainer = 2,
        [StringValue("[Дата отправления]")] DateSending = 3,
        [StringValue("[Вид перевозки]")] TransportationType = 4,
        [StringValue("[Номер документа]")] NumberDoc = 5,
        [StringValue("[Тоннажность контейнера]")] ContainerVolume = 8,
        [StringValue("[Код Груза]")] CargoCode = 6,
        [StringValue("[Наименование груза]")] CargoName = 10,
        [StringValue("[Тип груза]")] CargoGroupName = 48,
        [StringValue("[Тарифный класс груза]")] CargoClassName = 49,
        
        [StringValue("[Государство отправления]")] CountrySending = 11,
        [StringValue("[Дорога отправления]")] WaySending = 12,
        [StringValue("[Субъект РФ отправления]")] SubjectSendingRUS = 13,
        [StringValue("[Станция отправления РФ]")] StationSendingRUS = 36,
        [StringValue("[Станция отправления СНГ]")] StationSendingSNG = 37,
        [StringValue("[Код станции отправления РФ]")] StationSendingCodeRUS = 34,
        [StringValue("[Код станции отправления СНГ]")] StationSendingCodeSNG = 35,

        [StringValue("[Грузоотправитель наим по ЕГРПО]")] CompanySendingEGRPO = 43, 
        [StringValue("[Грузополучатель наим по ЕГРПО]")] CompanyRecipientERPGO = 44,

        [StringValue("[Грузоотправитель(код)]")] CompanySendingCode = 50,
        [StringValue("[Грузополучатель(код)]")] CompanyRecipientCode = 45,

        [StringValue("[Государство назначения]")] CountryDelivering = 20,
        [StringValue("[Дорога назначения]")] WayDelivering = 21,
        [StringValue("[Субъект РФ назначения]")] SubjectDeliveringRUS = 22,
        [StringValue("[Станция назначения РФ]")] StationDeliveringRUS = 40,
        [StringValue("[Станция назначения СНГ]")] StationDeliveringSNG = 41,
        [StringValue("[Код станции назначения РФ]")] StationDeliveringCodeRUS = 38,
        [StringValue("[Код станции назначения СНГ]")] StationDeliveringCodeSNG = 39,
        [StringValue("[Собственник вагона по ЕГРПО]")] OwnerVagonEGRPO = 29,
        [StringValue("[ОКПО]")] OKPO = 7,
        [StringValue("[Наименование станции отправления РФ/СНГ (код станции)]")]CODE_NAME_STATION_DELIVERING_RUS_SNG = 23,
        [StringValue("[Наименование станции назначения РФ/СНГ (код станции)]")]CODE_NAME_STATION_SENDING_RUS_SNG = 14,
        [StringValue("[Плательщик жд тарифа]")] PayerTariff=31,
        [StringValue("[Призн иск   изм перевозки]")] ISK_ChangeTransportation=666,
        [StringValue("[Арендатор вагона по ГВЦ]")] RenterVagonGVC = 30,
        [StringValue("[Вид подвижного состава]")] PodvigSostavType = 100,
        [StringValue("[Подрод Вагона]")] WagonType = 42,
        [StringValue("[Количество вагонов]")] QuantityVagon = 51,
        [StringValue("[Количество контейнеров]")] QuantityContainer = 52,
        [StringValue("Месяц перевозки")] MonthTransportation = 47,
        [StringValue("[Объем перевозок (тн)]")]
        VolumeTransportation = 333,
        [StringValue("[Провозная плата]")]
        PaymentSum = 222,
        [StringValue("[Ваг*Км (тариф Расстояние)]")]
        DestinationTariff = 111,
        [StringValue("[Тонно киллометры]")]
        TonnKm = 444,
        [StringValue("[Ранее перевозимый груз]")]
        CargoNameEarlyTransportation = 53,
    }
    
    /// <summary>
    ///     This attribute is used to represent a string value
    ///     for a value in an enum.
    /// </summary>
    public class StringValueAttribute : Attribute
    {
        #region Properties

        /// <summary>
        ///     Holds the stringvalue for a value in an enum.
        /// </summary>
        public string StringValue { get; protected set; }

        #endregion

        #region Constructor

        /// <summary>
        ///     Constructor used to init a StringValue Attribute
        /// </summary>
        /// <param name="value"></param>
        public StringValueAttribute(string value)
        {
            StringValue = value;
        }

        #endregion
    }
}