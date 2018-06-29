using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SuccessHotelierHub.Models
{
    public class PaymentMethodEnum
    {

        public const string VisaDebit = "59B7A188-B794-4ACA-B00F-12613B822F52";
        public const string Cash = "58BC4291-177A-4258-B68A-22AEA58FF217";
        public const string Visa = "AB47B5AD-0C79-47E8-AC0F-799FD4872F3B";
        public const string AmericanExpress = "F5675C7B-397A-4349-8DFD-7D9421A2299E";
        public const string Discover = "9F19777A-C984-4770-870A-F271B448AEB7";
        public const string MasterCard = "099518D7-42B2-4055-BB8E-F99609E197C1";
    }

    enum PaymentEnum
    {
        [Description("59B7A188-B794-4ACA-B00F-12613B822F52")]
        Laser,
        [Description("58BC4291-177A-4258-B68A-22AEA58FF217")]
        Cash,

        [Description("AB47B5AD-0C79-47E8-AC0F-799FD4872F3B")]
        Visa,

        [Description("F5675C7B-397A-4349-8DFD-7D9421A2299E")]
        AmericanExpress,

        [Description("9F19777A-C984-4770-870A-F271B448AEB7")]
        Discover,

        [Description("099518D7-42B2-4055-BB8E-F99609E197C1")]
        MasterCard
    }

    public class AllPaymentMethod
    {
        public string[] Methods = { "59B7A188-B794-4ACA-B00F-12613B822F52",
                                  "58BC4291-177A-4258-B68A-22AEA58FF217",
                                    "AB47B5AD-0C79-47E8-AC0F-799FD4872F3B",
                                "F5675C7B-397A-4349-8DFD-7D9421A2299E",
                                    "9F19777A-C984-4770-870A-F271B448AEB7",
                                    "099518D7-42B2-4055-BB8E-F99609E197C1"};
    }
}