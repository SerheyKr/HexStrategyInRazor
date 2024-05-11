// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


const canvas = document.getElementById("canvas");
var currentTileId = "";
var sendArmyCount = "ALL";

let isAlerted = false;

let key = [];
window.addEventListener("keydown", function (e) {
	key[e.keyCode] = e.keyCode;
});
window.addEventListener("keyup", function (e) {
	key.splice(key.indexOf(e.keyCode), 1);
});


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
}

function updateData() {
	$.ajax({
		url: '/getData', // URL to your server endpoint to fetch data
		method: 'GET',
		success: function (data) {
			// Update the data container with new data
			$('#data-container').html(data);
		},
	});
}

function RestartMap()
{
	$.ajax({
		url: '/restartMap',
		method: 'POST',
		success: function (json) {
			UpdataMap(json);
		},
	});

	updateAllData();
}

function EndTurn()
{
	$.ajax({
		url: '/endTurn',
		method: 'POST',
		success: function (json) {
			UpdataMap(json);
		},
	});

	updateAllData();
}

function updateCellData() {
	$.ajax({
		url: '/getMapData', // URL to your server endpoint to fetch data
		method: 'GET',
		success: function (json) {
			UpdataMap(json);
		},
	});
}

function UpdataMap(json)
{
	let data = eval(JSON.parse(json.value));
	$('#TotalArmy').html("Total army: " + data.TotalArmy);

	data.Rows.forEach((element) => {
		element.Cells.forEach((cell) => {

			$(`#${cell.positionId}`).html(`U:${cell.unitsCount}`);

			$("." + cell.positionId + "hexBlock").each(function () {
				let backgrounds = [];

				backgrounds.push("url(../images/grid.svg)");

				if (cell.cellColorHTML == "Blue") {
					for (let i = 0; i < cell.sendArmyToPositionX.length; i++) {
						if (cell.positionY % 2 == 0) {
							switch (true) {
								case (cell.positionX - cell.sendArmyToPositionX[i] == 0 && cell.positionY - cell.sendArmyToPositionY[i] == -1):
									backgrounds.push("url(../images/Blue/gridRightBottom.svg)");
									break;
								case (cell.positionX - cell.sendArmyToPositionX[i] == -1 && cell.positionY - cell.sendArmyToPositionY[i] == 0):
									backgrounds.push("url(../images/Blue/gridBottom.svg)");
									break;
								case (cell.positionX - cell.sendArmyToPositionX[i] == 0 && cell.positionY - cell.sendArmyToPositionY[i] == 1):
									backgrounds.push("url(../images/Blue/gridLeftBottom.svg)");
									break;
								case (cell.positionX - cell.sendArmyToPositionX[i] == 1 && cell.positionY - cell.sendArmyToPositionY[i] == 1):
									backgrounds.push("url(../images/Blue/gridTopLeft.svg)");
									break;
								case (cell.positionX - cell.sendArmyToPositionX[i] == 1 && cell.positionY - cell.sendArmyToPositionY[i] == 0):
									backgrounds.push("url(../images/Blue/gridTop.svg)");
									break;
								case (cell.positionX - cell.sendArmyToPositionX[i] == 1 && cell.positionY - cell.sendArmyToPositionY[i] == -1):
									backgrounds.push("url(../images/Blue/gridTopRight.svg)");
									break;
							}
						} else {
							switch (true) {
								case (cell.positionX - cell.sendArmyToPositionX[i] == 0 && cell.positionY - cell.sendArmyToPositionY[i] == -1):
									backgrounds.push("url(../images/Blue/gridTopRight.svg)");
									break;
								case (cell.positionX - cell.sendArmyToPositionX[i] == -1 && cell.positionY - cell.sendArmyToPositionY[i] == -1):
									backgrounds.push("url(../images/Blue/gridRightBottom.svg)");
									break;
								case (cell.positionX - cell.sendArmyToPositionX[i] == -1 && cell.positionY - cell.sendArmyToPositionY[i] == 0):
									backgrounds.push("url(../images/Blue/gridBottom.svg)");
									break;
								case (cell.positionX - cell.sendArmyToPositionX[i] == -1 && cell.positionY - cell.sendArmyToPositionY[i] == 1):
									backgrounds.push("url(../images/Blue/gridLeftBottom.svg)");
									break;
								case (cell.positionX - cell.sendArmyToPositionX[i] == 0 && cell.positionY - cell.sendArmyToPositionY[i] == 1):
									backgrounds.push("url(../images/Blue/gridTopLeft.svg)");
									break;
								case (cell.positionX - cell.sendArmyToPositionX[i] == 1 && cell.positionY - cell.sendArmyToPositionY[i] == 0):
									backgrounds.push("url(../images/Blue/gridTop.svg)");
									break;
							}
						}
					}
				} else if (cell.cellColorHTML == "Red") {

					for (let i = 0; i < cell.sendArmyToPositionX.length; i++) {
						if (cell.positionY % 2 == 0) {
							switch (true) {
								case (cell.positionX - cell.sendArmyToPositionX[i] == 0 && cell.positionY - cell.sendArmyToPositionY[i] == -1):
									backgrounds.push("url(../images/Red/gridRightBottom.svg)");
									break;
								case (cell.positionX - cell.sendArmyToPositionX[i] == -1 && cell.positionY - cell.sendArmyToPositionY[i] == 0):
									backgrounds.push("url(../images/Red/gridBottom.svg)");
									break;
								case (cell.positionX - cell.sendArmyToPositionX[i] == 0 && cell.positionY - cell.sendArmyToPositionY[i] == 1):
									backgrounds.push("url(../images/Red/gridLeftBottom.svg)");
									break;
								case (cell.positionX - cell.sendArmyToPositionX[i] == 1 && cell.positionY - cell.sendArmyToPositionY[i] == 1):
									backgrounds.push("url(../images/Red/gridTopLeft.svg)");
									break;
								case (cell.positionX - cell.sendArmyToPositionX[i] == 1 && cell.positionY - cell.sendArmyToPositionY[i] == 0):
									backgrounds.push("url(../images/Red/gridTop.svg)");
									break;
								case (cell.positionX - cell.sendArmyToPositionX[i] == 1 && cell.positionY - cell.sendArmyToPositionY[i] == -1):
									backgrounds.push("url(../images/Red/gridTopRight.svg)");
									break;
							}
						} else {
							switch (true) {
								case (cell.positionX - cell.sendArmyToPositionX[i] == 0 && cell.positionY - cell.sendArmyToPositionY[i] == -1):
									backgrounds.push("url(../images/Red/gridTopRight.svg)");
									break;
								case (cell.positionX - cell.sendArmyToPositionX[i] == -1 && cell.positionY - cell.sendArmyToPositionY[i] == -1):
									backgrounds.push("url(../images/Red/gridRightBottom.svg)");
									break;
								case (cell.positionX - cell.sendArmyToPositionX[i] == -1 && cell.positionY - cell.sendArmyToPositionY[i] == 0):
									backgrounds.push("url(../images/Red/gridBottom.svg)");
									break;
								case (cell.positionX - cell.sendArmyToPositionX[i] == -1 && cell.positionY - cell.sendArmyToPositionY[i] == 1):
									backgrounds.push("url(../images/Red/gridLeftBottom.svg)");
									break;
								case (cell.positionX - cell.sendArmyToPositionX[i] == 0 && cell.positionY - cell.sendArmyToPositionY[i] == 1):
									backgrounds.push("url(../images/Red/gridTopLeft.svg)");
									break;
								case (cell.positionX - cell.sendArmyToPositionX[i] == 1 && cell.positionY - cell.sendArmyToPositionY[i] == 0):
									backgrounds.push("url(../images/Red/gridTop.svg)");
									break;
							}
						}
					}
				}
				$(this).css("background-image", backgrounds.join(", "))
			})

			let element = document.getElementById(cell.positionId);
			element.style.color = cell.cellColorHTML;
		});
	});
	$('#TotalTurns').html("Turns count: " + data.TurnsCount);

	if (data.IsEnded) {
		if (!isAlerted) {
			swal(data.EndText);
			document.getElementById("endTurnButton").style.display = "none";
		}
		isAlerted = true;
	} else {
		isAlerted = false;
		document.getElementById("endTurnButton").style.display = "inline-block";
	}
}

function sendArmyMoveData(FromId, ToId, Count)
{
	const dataToSend = JSON.stringify({ "FromId": FromId, "ToId": ToId, "ArmyCount": Count });

	$.ajax({
		url: '/sendArmyData',
		method: 'POST',
		data: dataToSend,
		success: function (json) {
			UpdataMap(json);
		},
	});
}

$(window).click(function (e) {
	let x = e.clientX, y = e.clientY,
		elementMouseIsOver = document.elementFromPoint(x, y);
		
	if (currentTileId.id && elementMouseIsOver.id)
	{
		let armyCount = 1;
		//console.log(currentTileId.id + "->" + elementMouseIsOver.id + "(" + armyCount + ")");
		sendArmyMoveData(currentTileId.id, elementMouseIsOver.id, armyCount);

		currentTileId = "";
	}
});

function updateRatio()
{
	let w = window.innerWidth;
	let h = window.innerHeight;
	//600: 500
	let ratio = 1;
	if (w < h) {
		ratio = w / 600 * 0.4;
	} else
	{
		ratio = h / 500 * 0.4;
	}
	console.log(`${w}: ${h} (${ratio})`);
	document.documentElement.style.setProperty('--ratio', ratio)
}

updateCellData();

updateRatio();

window.addEventListener('resize', updateRatio);