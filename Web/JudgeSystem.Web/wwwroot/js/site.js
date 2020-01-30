//Show error or info message
let infoBox = $('#infoBox');
let errorBox = $('#errorBox');

infoBox.on('click', () => {
	infoBox.hide();
});

errorBox.on('click', () => {
	errorBox.hide();
});

if (infoBox.find('span').text().trim() !== "") {
	infoBox.show();
	setTimeout(function () {
		$('#infoBox').fadeOut();
    }, 4000);
}

if (errorBox.find('span').text() !== "") {
    errorBox.show();
}