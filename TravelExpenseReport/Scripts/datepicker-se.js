(function (factory) {
    if (typeof define === "function" && define.amd) {

        // AMD. Register as an anonymous module.
        define(["../widgets/datepicker"], factory);
    } else {

        // Browser globals
        factory(jQuery.datepicker);
    }
}(function (datepicker) {

    datepicker.regional.se = {
        closeText: "Stäng",
        prevText: "&#xAB;Föregående",
        nextText: "Nästa&#xBB;",
        currentText: "I dag",
        monthNames: [
            "januari",
            "februari",
            "mars",
            "april",
            "maj",
            "juni",
            "juli",
            "augusti",
            "september",
            "oktober",
            "november",
            "december"
        ],
        monthNamesShort: ["jan", "feb", "mar", "apr", "maj", "jun", "jul", "aug", "sep", "okt", "nov", "dec"],
        dayNamesShort: ["sön", "mån", "tis", "ons", "tor", "fre", "lör"],
        dayNames: ["söndag", "måndag", "tisdag", "onsdag", "torsdag", "fredag", "lördag"],
        dayNamesMin: ["sö", "må", "ti", "on", "to", "fr", "lö"],
        weekHeader: "vecka",
        dateFormat: "dd.mm.yy",
        firstDay: 1,
        isRTL: false,
        showMonthAfterYear: false,
        yearSuffix: ""
    };
    datepicker.setDefaults(datepicker.regional.se);

    return datepicker.regional.se;

}));