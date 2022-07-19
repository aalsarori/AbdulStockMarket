var inv = 0;
var money = 10000;
var chart = anychart.column();
var chart2 = anychart.column();


document.getElementById("buyVariable").value = document.getElementById("mySlider").value;

function updateTextInput(val) {
    document.getElementById('buyVariable').value = val;
    inv = val;
    money = 10000 - inv;
    //anychart.onDocumentReady;
    anychart.onDocumentReady(function () {


        // set the data
        var data = {
            header: ["Name", "US Dollars"],
            rows: [
                ["Invest", inv],
                ["Money Left", money]
            ]
        };

        // create the chart


        // add data
        chart.data(data);

        // set the chart title
        chart.title("Stock invest");

        // draw
        chart.container("container");
        chart.draw();
    });
}

function updateSliderInput(val) {
    document.getElementById('mySlider').value = val;

    document.getElementById('buyVariable').value = val;
    inv = val;
    money = 10000 - inv;
    //anychart.onDocumentReady;
    anychart.onDocumentReady(function () {


        // set the data
        var data = {
            header: ["Name", "US Dollars"],
            rows: [
                ["Invest", inv],
                ["Money Left", money]
            ]
        };

        // create the chart


        // add data
        chart.data(data);

        // set the chart title
        chart.title("Stock invest");

        // draw
        chart.container("container");
        chart.draw();
    });
    
}


document.getElementById("sellVariable").value = document.getElementById("mySlider2").value;

function updateTextInput1(val) {
    document.getElementById('sellVariable').value = val;
    inv = val;
    money = 10000 - inv;
    //anychart.onDocumentReady;
    anychart.onDocumentReady(function () {


        // set the data
        var data = {
            header: ["Name", "US Dollars"],
            rows: [
                ["Invest", inv],
                ["Money Left", money]
            ]
        };

        // create the chart


        // add data
        chart2.data(data);

        // set the chart title
        chart2.title("Stock invest");

        // draw
        chart2.container("container2");
        chart2.draw();
    });
}

function updateSliderInput1(val) {
    document.getElementById('mySlider2').value = val;

    document.getElementById('sellVariable').value = val;
    inv = val;
    money = 10000 - inv;
    //anychart.onDocumentReady;
    anychart.onDocumentReady(function () {


        // set the data
        var data = {
            header: ["Name", "US Dollars"],
            rows: [
                ["Invest", inv],
                ["Money Left", money]
            ]
        };

        // create the chart


        // add data
        chart2.data(data);

        // set the chart title
        chart2.title("Stock invest");

        // draw
        chart2.container("container2");
        chart2.draw();
    });

}



(function () {

    function ready(fn) {
        if (document.attachEvent ? document.readyState === "complete" : document.readyState !== "loading") {
            fn();
        } else {
            document.addEventListener('DOMContentLoaded', fn);
        }
    }

    ready(setupPieChart);


    function setupPieChart() {


        var dimensions = knuthfisheryates2(['Money', 'Invest']);
        var randomProportions = generateRandomProportions(dimensions.length, 0.05);
        var proportions = dimensions.map(function (d, i) {
            return {
                label: d,
                proportion: randomProportions[i],
                collapsed: false,
                format: {
                    label: d.charAt(0).toUpperCase() + d.slice(1) // capitalise first letter
                }
            }
        });


        var setup = {
            canvas: document.getElementById('piechart'),
            radius: 0.9,
            collapsing: true,
            proportions: proportions,
            drawSegment: drawSegmentOutlineOnly,
            onchange: onPieChartChange
        };

        var newPie = new DraggablePiechart(setup);

        function drawSegmentOutlineOnly(context, piechart, centerX, centerY, radius, startingAngle, arcSize, format, collapsed) {

            if (collapsed) { return; }

            // Draw segment
            context.save();
            var endingAngle = startingAngle + arcSize;
            context.beginPath();
            context.moveTo(centerX, centerY);
            context.arc(centerX, centerY, radius, startingAngle, endingAngle, false);
            context.closePath();

            context.fillStyle = '#f5f5f5';
            context.fill();
            context.stroke();
            context.restore();

            // Draw label on top
            context.save();
            context.translate(centerX, centerY);
            context.rotate(startingAngle);

            var fontSize = Math.floor(context.canvas.height / 25);
            var dx = radius - fontSize;
            var dy = centerY / 10;

            context.textAlign = "right";
            context.font = fontSize + "pt Helvetica";
            context.fillText(format.label, dx, dy);
            context.restore();
        }



        function onPieChartChange(piechart) {

            var table = document.getElementById('proportions-table');
            var percentages = piechart.getAllSliceSizePercentages();

            // percentages[0] = money;
            // percentages[1] = inv;


            var labelsRow = '<tr>';
            var propsRow = '<tr>';

            updateTextInput(percentages[0].toFixed(0) * 100);
            updateSliderInput(percentages[0].toFixed(0) * 100);

            updateTextInput1(percentages[0].toFixed(0) * 100);
            updateSliderInput1(percentages[0].toFixed(0) * 100);

            for (var i = 0; i < proportions.length; i += 1) {
                labelsRow += '<th>' + proportions[i].format.label + '</th>';

                var v = '<var>$' + percentages[i].toFixed(0) * 100 + '</var>';
                var plus = '<div id="plu-' + dimensions[i] + '" class="adjust-button" data-i="' + i + '" data-d="-1">&#43;</div>';
                var minus = '<div id="min-' + dimensions[i] + '" class="adjust-button" data-i="' + i + '" data-d="1">&#8722;</div>';
                propsRow += '<td>' + v + plus + minus + '</td>';
            }
            labelsRow += '</tr>';
            propsRow += '</tr>';

            table.innerHTML = labelsRow + propsRow;

            var adjust = document.getElementsByClassName("adjust-button");

            function adjustClick(e) {
                var i = this.getAttribute('data-i');
                var d = this.getAttribute('data-d');

                piechart.moveAngle(i, (d * 0.1));
            }

            for (i = 0; i < adjust.length; i++) {
                adjust[i].addEventListener('click', adjustClick);
            }

        }

        /*
         * Generates n proportions with a minimum percentage gap between them
         */
        function generateRandomProportions(n, min) {

            // n random numbers 0 - 1
            var rnd = Array.apply(null, { length: n }).map(function () { return Math.random(); });

            // sum of numbers
            var rndTotal = rnd.reduce(function (a, v) { return a + v; }, 0);

            // get proportions, then make sure each propoertion is above min
            return validateAndCorrectProportions(rnd.map(function (v) { return v / rndTotal; }), min);


            function validateAndCorrectProportions(proportions, min) {
                var sortedProportions = proportions.sort(function (a, b) { return a - b });

                for (var i = 0; i < sortedProportions.length; i += 1) {
                    if (sortedProportions[i] < min) {
                        var diff = min - sortedProportions[i];
                        sortedProportions[i] += diff;
                        sortedProportions[sortedProportions.length - 1] -= diff;
                        return validateAndCorrectProportions(sortedProportions, min);
                    }
                }

                return sortedProportions;
            }
        }

        /*
         * Array sorting algorithm
         */
        function knuthfisheryates2(arr) {
            var temp, j, i = arr.length;
            while (--i) {
                j = ~~(Math.random() * (i + 1));
                temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
            }

            return arr;
        }

    }

})();




// 



