// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//let x = 0;
//let y = 0;
//let key = [];

//window.addEventListener("keypress", function (event) {
//    //if (event.defaultPrevented) {
//    //    return; // Do nothing if the event was already processed
//    //}
//    //this.alert(event.key);
//    switch (event.key) {
//        case "s":
//            y += 1;
//            break;
//        case "w":
//            y -= 1;
//            break;
//        case "a":
//            x -= 1;
//            break;
//        case "d":
//            x += 1;
//            break;
//        default:
//            return; // Quit when this doesn't handle the key event.
//    }

//    // Cancel the default action to avoid it being handled twice
//    //event.preventDefault();
//}, true);

//function loop() {
//    let mes = "";
//    let l = key.length;
//    for (let i = 0; i < l; i++) {
//        if (key[i]) {
//            mes += i.toString();
//        }
//    }

//    if (l != 0)
//        console.log(mes);
//    setTimeout(loop, 1000 / 24);
//}

const canvas = document.getElementById("canvas");
const context = canvas.getContext("2d");

let key = [];
window.addEventListener("keydown", function (e) {
	key[e.keyCode] = e.keyCode;
});
window.addEventListener("keyup", function (e) {
	key.splice(key.indexOf(e.keyCode), 1);
});

//let gameWidth = window.innerWidth
//let gameHeight = window.innerHeight
//let ratio = 1.5
//if (gameHeight / gameWidth < ratio) {
//	gameWidth = Math.ceil(gameHeight / ratio)
//}
//$('.content').css({ "height": gameHeight + "px", "width": gameWidth + "px" })
//$('.js-modal-content').css({ "width": gameWidth + "px" })

//window.addEventListener("keypress", function (event) {
//	key[e.keyCode] = e.type;
//});


function Controll(key, context)
{
	var y = 0, l = key.length, i, t;

	for (i = 0; i < l; i++) {
		if (key[i]) {
			t = i + ' (' + i.toString() + ')';
			context.fillText(t, canvas.width / 2, y);
			y += 22;
		}
	}
}

function Draw()
{
	context.font = 'bold 22px Courier New';
	context.textBaseline = 'top';
	context.textAlign = 'center';

	(function loop() {
		context.clearRect(0, 0, canvas.width, canvas.height);
		Controll(key, context);
		//context.fillRect(0, 0, 5000, 5000);
		

		setTimeout(loop, 1000 / 24);
	})();

	//canvas.onkeydown = canvas.onkeyup = function (e) {
	//    var e = e || event;
	//    key[e.keyCode] = e.type == 'keydown';
	//};
}

//function drawTimer()
//{
//    let timer = setInterval(function () {
//        draw();
//        loop();
//        //console.log("HIO");
//    }, 20);
//}

function updateData() {
	$.ajax({
		url: '/getData', // URL to your server endpoint to fetch data
		method: 'GET',
		success: function (data) {
			// Update the data container with new data
			$('#data-container').html(data);
		},
		error: function (xhr, status, error) {
			console.error('Error fetching data:', error);
		}
	});
}

function updateCellData() {
	$.ajax({
		url: '/getData', // URL to your server endpoint to fetch data
		method: 'GET',
		success: function (data) {
			// Update the data container with new data
			$('#data-container').html(data);
		},
		error: function (xhr, status, error) {
			console.error('Error fetching data:', error);
		}
	});
}

updateData();

setInterval(updateData, 1000); // 5000 milliseconds = 5 seconds