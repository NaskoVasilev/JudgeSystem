function initializeClock() {
	let secondsSpan = document.getElementById("seconds");
	let minutesSpan = document.getElementById("minutes");
	let hoursSpan = document.getElementById("hours");
	let daysSpan = document.getElementById("days");


	let seconds = Number.parseInt(secondsSpan.textContent);
	let minutes = Number.parseInt(minutesSpan.textContent);
	let hours = Number.parseInt(hoursSpan.textContent);
	let days = Number.parseInt(daysSpan.textContent);

	var timeinterval = setInterval(updateClock, 1000);
	updateClock();

	function updateClock() {
		seconds--;
		if (seconds < 0) {
			seconds = 59;
			minutes--;
			if (minutes < 0) {
				minutes = 59;
				hours--;
				if (hours < 0) {
					hours = 11;
					days--;
					if (days <= 0) {
						seconds = 0;
						hours = 0;
						days = 0;
						minutes = 0;
						clearInterval(timeinterval);
					}
				}
			}
		}

		daysSpan.innerHTML = days;
		hoursSpan.innerHTML = ('0' + hours).slice(-2);
		minutesSpan.innerHTML = ('0' + minutes).slice(-2);
		secondsSpan.innerHTML = ('0' + seconds).slice(-2);
	}
}
