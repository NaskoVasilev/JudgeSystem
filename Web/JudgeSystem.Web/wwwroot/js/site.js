//Show error or info message
let infoBox = $('#infoBox');
let errorBox = $('#errorBox');

infoBox.on('click', () => {
	infoBox.hide();
});

errorBox.on('click', () => {
	errorBox.hide();
});
if (infoBox.find('span').text() !== "") {
	infoBox.show();
	setTimeout(function () {
		$('#infoBox').fadeOut();
	}, 3000);
}

if (errorBox.find('span').text() !== "") {
	errorBox.show();
}