function chartCreateAndUpdate(chartID, strChartTmp, chartType) {
    debugger;
    try {
        if (chartType == "" || chartType == "0" || chartType == null) {            
            chartType = "";
        }

        var aryChartTmp = strChartTmp;

        var Series = new Array();
        var SeriesLabel = new Array();

        var XVal = new Array();
        var YVal = new Array();        
        var ZVal = new Array();

        var labelCtr = 0;
        var seriesCtr = 0;
        var startYear;
        var pointInt = 0;
        var YAaxisName = "";
        var ZAaxisName = "";

        if (aryChartTmp != "") {

            if (chartID == "divChart") {                
                for (var m = 0; m < strChartTmp.length; m++) {                    
                    XVal[m] = strChartTmp[m].UserName;
                    YVal[m] = strChartTmp[m].Seen;
                    ZVal[m] = strChartTmp[m].FromIP;
                }
            }
            if (chartID == "divChart") {             
                pointInt = 171999999;
                YAaxisName = "Seen";
                ZAaxisName = "From IP";
            }            
            var chartObj = null;
           
            
            if (chartType == "" || chartType == "0" || chartType == null) {
                chartObj = $("#" + chartID).highcharts();             
            }
            else {
                chartObj = $("#" + chartID).highcharts({ chart: { type: chartType, renderTo: 'container', } });
            }

            var xxx = ["aa", "aa", "aa", "aa", "aa", "aa", "aa", "aa"];
           if (chartID == 'divChart') {           
               chartObj = $("#" + chartID).highcharts(
                    {
                        chart: {
                            type: chartType,
                            renderTo: 'container'                            
                        },
                        title: {
                            text: 'User Connection'
                        },
                        xAxis: [{
                            min: 0,
                            max: 7,
                            categories: XVal,
                            labels: {
                                rotation: -55,
                                style: {
                                    fontSize: '13px',
                                    fontFamily: 'Verdana, sans-serif'
                                }
                            }                            
                        }],
                        yAxis: [{ // Primary yAxis - for Implied Rate set in value
                            labels: {
                                format: '{value}',
                                style: {
                                    color: Highcharts.getOptions().colors[1]
                                }
                            },
                            title: {
                                text: 'Seen',
                                style: {
                                    color: Highcharts.getOptions().colors[1]
                                }
                            },
                            opposite: false

                        }, {
                            gridLineWidth: 0,
                            title: {
                                text: '',                                
                            },
                            labels: {
                                format: '{value}',                                
                            },
                            opposite: false
                        }],
                        tooltip: {
                            shared: true
                        },
                        legend: {
                            layout: 'horizontal',
                            align: 'left',
                            x: 150,
                            verticalAlign: 'top',
                            y: 20,
                            floating: true,
                            backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'                            
                        },
                        scrollbar: {
                            enabled: true
                        },
                        series: [{
                            name: 'Seen',                            
                            type: chartType,
                            data: YVal                            
                        }]
                    });
            }                    
        }
        else {
            var chartObj = $("#" + chartID).highcharts();
            for (var iCtr = 0; iCtr < chartObj.series.length; iCtr++) {
                chartObj.series[iCtr].remove(true);
            }

        }
    }

    catch (errorVal) {
          //alert( "chartCreateAndUpdate : " + errorVal );
    }
    finally {
        Series = null;
        SeriesLabel = null;        
    }
}






