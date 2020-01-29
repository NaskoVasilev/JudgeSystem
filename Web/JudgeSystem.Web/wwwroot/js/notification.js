function showInfo(message) {
	let infoBox = $('#infoBox');
	infoBox.find('div > span').text(message);
	infoBox.show();
	setTimeout(function () {
		$('#infoBox').fadeOut();
    }, 4000);
}

function showError(err) {
	let errorBox = $('#errorBox');
	errorBox.find('div > span').text(err);
	errorBox.show();
}
