﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
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
	});

	updateAllData();
}

function EndTurn()
{
	$.ajax({
		url: '/endTurn',
		method: 'POST',
	});

	updateAllData();
}

function updateCellData() {
	$.ajax({
		url: '/getMapData', // URL to your server endpoint to fetch data
		method: 'GET',
		success: function (json) {
			let data = eval(JSON.parse(json.value));

			data.Rows.forEach((element) => {
				element.Cells.forEach((cell) => {

					$(`#${cell.positionId}`).html(`U:${cell.unitsCount} <br />C:${cell.controllerName}`);

					$("." + cell.positionId + "hexBlock").each(function () {
						let backgrounds = [];

						backgrounds.push("url(../images/grid.svg)");

						if (cell.cellColorHTML == "Blue") {
							for (let i = 0; i < cell.sendArmyToPositionX.length; i++) {
								if (cell.positionY % 2 == 0) {
									if (cell.positionX - cell.sendArmyToPositionX[i] == 0 && cell.positionY - cell.sendArmyToPositionY[i] == -1) {
										backgrounds.push("url(../images/Blue/gridRightBottom.svg)");
									}
									if (cell.positionX - cell.sendArmyToPositionX[i] == -1 && cell.positionY - cell.sendArmyToPositionY[i] == 0) {
										backgrounds.push("url(../images/Blue/gridBottom.svg)");
									}
									if (cell.positionX - cell.sendArmyToPositionX[i] == 0 && cell.positionY - cell.sendArmyToPositionY[i] == 1) {
										backgrounds.push("url(../images/Blue/gridLeftBottom.svg)");
									}
									if (cell.positionX - cell.sendArmyToPositionX[i] == 1 && cell.positionY - cell.sendArmyToPositionY[i] == 1) {
										backgrounds.push("url(../images/Blue/gridTopLeft.svg)");
									}
									if (cell.positionX - cell.sendArmyToPositionX[i] == 1 && cell.positionY - cell.sendArmyToPositionY[i] == 0) {
										backgrounds.push("url(../images/Blue/gridTop.svg)");
									}
									if (cell.positionX - cell.sendArmyToPositionX[i] == 1 && cell.positionY - cell.sendArmyToPositionY[i] == -1) {
										backgrounds.push("url(../images/Blue/gridTopRight.svg)");
									}
								} else {
									if (cell.positionX - cell.sendArmyToPositionX[i] == 0 && cell.positionY - cell.sendArmyToPositionY[i] == -1) {
										backgrounds.push("url(../images/Blue/gridTopRight.svg)");
									}
									if (cell.positionX - cell.sendArmyToPositionX[i] == -1 && cell.positionY - cell.sendArmyToPositionY[i] == -1) {
										backgrounds.push("url(../images/Blue/gridRightBottom.svg)");
									}
									if (cell.positionX - cell.sendArmyToPositionX[i] == -1 && cell.positionY - cell.sendArmyToPositionY[i] == 0) {
										backgrounds.push("url(../images/Blue/gridBottom.svg)");
									}
									if (cell.positionX - cell.sendArmyToPositionX[i] == -1 && cell.positionY - cell.sendArmyToPositionY[i] == 1) {
										backgrounds.push("url(../images/Blue/gridLeftBottom.svg)");
									}
									if (cell.positionX - cell.sendArmyToPositionX[i] == 0 && cell.positionY - cell.sendArmyToPositionY[i] == 1) {
										backgrounds.push("url(../images/Blue/gridTopLeft.svg)");
									}
									if (cell.positionX - cell.sendArmyToPositionX[i] == 1 && cell.positionY - cell.sendArmyToPositionY[i] == 0) {
										backgrounds.push("url(../images/Blue/gridTop.svg)");
									}
								}
							}
						} else if (cell.cellColorHTML == "Red") {

							for (let i = 0; i < cell.sendArmyToPositionX.length; i++) {
								if (cell.positionY % 2 == 0) {
									if (cell.positionX - cell.sendArmyToPositionX[i] == 0 && cell.positionY - cell.sendArmyToPositionY[i] == -1) {
										backgrounds.push("url(../images/Red/gridRightBottom.svg)");
									}
									if (cell.positionX - cell.sendArmyToPositionX[i] == -1 && cell.positionY - cell.sendArmyToPositionY[i] == 0) {
										backgrounds.push("url(../images/Red/gridBottom.svg)");
									}
									if (cell.positionX - cell.sendArmyToPositionX[i] == 0 && cell.positionY - cell.sendArmyToPositionY[i] == 1) {
										backgrounds.push("url(../images/Red/gridLeftBottom.svg)");
									}
									if (cell.positionX - cell.sendArmyToPositionX[i] == 1 && cell.positionY - cell.sendArmyToPositionY[i] == 1) {
										backgrounds.push("url(../images/Red/gridTopLeft.svg)");
									}
									if (cell.positionX - cell.sendArmyToPositionX[i] == 1 && cell.positionY - cell.sendArmyToPositionY[i] == 0) {
										backgrounds.push("url(../images/Red/gridTop.svg)");
									}
									if (cell.positionX - cell.sendArmyToPositionX[i] == 1 && cell.positionY - cell.sendArmyToPositionY[i] == -1) {
										backgrounds.push("url(../images/Red/gridTopRight.svg)");
									}
								} else {
									if (cell.positionX - cell.sendArmyToPositionX[i] == 0 && cell.positionY - cell.sendArmyToPositionY[i] == -1) {
										backgrounds.push("url(../images/Red/gridTopRight.svg)");
									}
									if (cell.positionX - cell.sendArmyToPositionX[i] == -1 && cell.positionY - cell.sendArmyToPositionY[i] == -1) {
										backgrounds.push("url(../images/Red/gridRightBottom.svg)");
									}
									if (cell.positionX - cell.sendArmyToPositionX[i] == -1 && cell.positionY - cell.sendArmyToPositionY[i] == 0) {
										backgrounds.push("url(../images/Red/gridBottom.svg)");
									}
									if (cell.positionX - cell.sendArmyToPositionX[i] == -1 && cell.positionY - cell.sendArmyToPositionY[i] == 1) {
										backgrounds.push("url(../images/Red/gridLeftBottom.svg)");
									}
									if (cell.positionX - cell.sendArmyToPositionX[i] == 0 && cell.positionY - cell.sendArmyToPositionY[i] == 1) {
										backgrounds.push("url(../images/Red/gridTopLeft.svg)");
									}
									if (cell.positionX - cell.sendArmyToPositionX[i] == 1 && cell.positionY - cell.sendArmyToPositionY[i] == 0) {
										backgrounds.push("url(../images/Red/gridTop.svg)");
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
			} else
			{
				isAlerted = false;
				document.getElementById("endTurnButton").style.display = "inline-block";
			}
		},
	});
}

function updateUserData() {
	$.ajax({
		url: '/getUserData', // URL to your server endpoint to fetch data
		method: 'GET',
		success: function (json) {
			let data = eval(JSON.parse(json.value));
			/*$('#Monies').html("Current monies: " + data.Monies);*/
			$('#TotalArmy').html("Total army: " + data.TotalArmy);
			/*$('#TotalBuildings').html("Total buildings: " + data.TotalBuildings);*/
		},
	});
}

function sendArmyMoveData(FromId, ToId, Count)
{
	const dataToSend = JSON.stringify({ "FromId": FromId, "ToId": ToId, "ArmyCount": Count });

	$.ajax({
		url: '/sendArmyData',
		method: 'POST',
		data: dataToSend,
	});

	updateCellData();
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

function updateAllData()
{
	updateCellData();
	updateUserData();
}

updateAllData();

setInterval(updateAllData, 500); // 5000 milliseconds = 5 seconds