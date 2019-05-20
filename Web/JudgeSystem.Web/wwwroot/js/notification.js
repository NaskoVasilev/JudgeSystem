function showInfo(message) {
	let infoBox = $('#infoBox');
	infoBox.find('span').text(message);
	infoBox.show();
	setTimeout(function () {
		$('#infoBox').fadeOut();
	}, 3000);
}

function showError(err) {
	let errorBox = $('#errorBox');
	errorBox.find('span').text(err);
	errorBox.show();
}
