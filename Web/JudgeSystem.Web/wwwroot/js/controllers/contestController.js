$('#course').change((e) => {
	var value = $(e.target).children("option:selected").val();
	$.get("/Administration/Contest/GetLessons?courseId=" + value)
		.done(lessons => {
			let lessonsSelectList = $('#lessonId');
			lessonsSelectList.disabled = false;

			for (let lesson of lessons) {
				lessonsSelectList.append($(`<option value="${lesson.id}">${lesson.name}</option>`));
			}
		})
		.fail((error) => {
			console.log(error);
		});
});

$('#lessonId').change((e) => {
	console.log($('#add-contest'));
	$($('#add-contest')[0]).removeAttr('disabled');
});
