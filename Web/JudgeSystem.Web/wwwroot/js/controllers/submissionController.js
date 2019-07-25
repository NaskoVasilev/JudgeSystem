let submissiosPerPage = 5;
let currentPageClass = 'current-page';
let zipFileSubmissionType = "ZipFile";

window.onload = () => {
	let hash = window.location.hash;

	let currentElement = $(`li.problem-name:contains(${decodeURI(hash).substr(1)})`)[0];
	if (currentElement) {
		$(".problem-name").removeClass("active-problem");
        $(currentElement).addClass("active-problem");
        let submissionType = currentElement.dataset.type.toString();
        hideOneOfCodeInputs(submissionType);
	}
	else {
		$('li.problemName')[0].classList.add('active-problem');
	}

	$(".active-problem").click();

	let problemId = $('.active-problem')[0].dataset.id;
	genratePaginationPages(problemId);
};

$(".problem-name").on("click", (e) => {

	let oldId = $(".active-problem")[0].dataset.id;
	$(".active-problem").removeClass("active-problem");
    $(e.target).addClass("active-problem");

    let submissionType = e.target.dataset.type.toString();
    hideOneOfCodeInputs(submissionType);

	window.location.hash = e.target.textContent;

	let id = $(e.target)[0].dataset.id;

	//If we refresh the id and oldId will be get from the same elemnt and in this case the oldId 
	//must be set to the id of the first li with class problem- name
	if (id === oldId) {
		oldId = $('li.problem-name')[0].dataset.id;
	}

	$('#problemName')[0].innerText = e.target.textContent;

	if ($("#admin-buttons").length > 0) {
		let editProblemBtnHrefAttribute = $('#editProblemBtn').attr('href');
		editProblemBtnHrefAttribute = editProblemBtnHrefAttribute.replace(`/${oldId}`, `/${id}`);
		$('#editProblemBtn').attr('href', editProblemBtnHrefAttribute);

		let deleteProblemBtnHrefAttribute = $('#deleteProblemBtn').attr('href');
		deleteProblemBtnHrefAttribute = deleteProblemBtnHrefAttribute.replace(`/${oldId}`, `/${id}`);
		$('#deleteProblemBtn').attr('href', deleteProblemBtnHrefAttribute);

		let addTestHrefAttribute = $('#addTestBtn').attr('href');
		addTestHrefAttribute = addTestHrefAttribute.replace(`problemId=${oldId}`, `problemId=${id}`);
		$('#addTestBtn').attr('href', addTestHrefAttribute);

		let allTestsBtnHrefAttribute = $('#allTestsBtn').attr('href');
		allTestsBtnHrefAttribute = allTestsBtnHrefAttribute.replace(`problemId=${oldId}`, `problemId=${id}`);
		$('#allTestsBtn').attr('href', allTestsBtnHrefAttribute);

	}
	
	let page = 1;
	genratePaginationPages(id);
	getSubmissions(id, page);

});

$('#submit-btn').on('click', () => {
	let problemId = $('.active-problem')[0].dataset.id;
    let submissionType = $('.active-problem')[0].dataset.type;
    let contestId = $('#submit-btn')[0].dataset.contestid;

    var formData = new FormData();
    formData.append('ProblemId', problemId);

    if (submissionType === zipFileSubmissionType) {
        formData.append('file', $('.file-upload-field')[0].files[0]);
    }
    else {
        let code = editor.getValue();
        formData.append('code', code);
    }

    if (contestId) {
        formData.append('contestId', contestId);
    }

    console.log(formData);

	let tr = $('<tr></tr>');
	let pointsTd = $('<td></td>');
	let spinner = $('<div class="spinner-border text-success" role="status"></div>');
	pointsTd.append(spinner);
    tr.append(pointsTd);

	let tbody = $('#submissions-holder tbody');
	tbody.prepend(tr);
	editor.setValue("");

    $.ajax({
        url: '/Submission/Create',
        data: formData,
        type: 'POST',
        contentType: false,
        processData: false
    })
        .done((response) => {
			$('#submissions-holder tbody tr:first-of-type').remove();
			let tr = generateTr(response);
			tbody.prepend(tr);

			let subbmissions = $('#submissions-holder > table > tbody > tr');
			if (subbmissions.length > submissiosPerPage) {
				for (let i = submissiosPerPage; i < subbmissions.length; i++) {
					subbmissions[i].remove();
				}
			}

			let currentPagesCount = $('.page-number').length;
			let problemId = $('.active-problem')[0].dataset.id;

			$.get(`/Submission/GetSubmissionsCount?problemId=${problemId}&contestId=${contestId}`)
				.done(submissionCount => {
					let pagesCount = Math.ceil(submissionCount / submissiosPerPage);
					if (pagesCount > currentPagesCount) {
						let nextButton = $('.pagination li:last-of-type');
						let newLi = $(`<li class="page-item page-number"><a class="page-link" href="#">${currentPagesCount + 1}</a></li>`);
						newLi.insertBefore(nextButton);

						$(newLi.children()[0]).on('click', (e) => {
							$("html, body").animate({ scrollTop: $(document).height() }, "slow");
							let page = e.target.innerText;
							let problemId = $('.active-problem')[0].dataset.id;
							getSubmissions(problemId, page);
						});
					}
				})
				.fail((error) => {
					console.log(error);
				});
		})
		.fail((error) => {
            showError(error.responseText);
            console.log("here")
            hideLoader();
		});
});

function genratePaginationPages(problemId) {
	let contestId = $('#submit-btn')[0].dataset.contestid;
	
    $.get(`/Submission/GetSubmissionsCount?problemId=${problemId}&contestId=${contestId}`)
		.done(submissionCount => {
			let pagesCount = Math.ceil(submissionCount / submissiosPerPage);
			if (pagesCount < 1) {
				pagesCount = 1;
			}
            $('.pagination .page-number').remove();
            let nextButton = $('.pagination li:last-of-type');
            for (var i = 1; i <= pagesCount; i++) {
                let li = $(`<li class="page-item page-number"><a class="page-link" href="#">${i}</a></li>`);
                li.insertBefore(nextButton);
            }
            $($('ul li.page-number > a')[0]).addClass(currentPageClass);
            $('.page-number > a').on('click', (e) => {
                $("html, body").animate({ scrollTop: $(document).height() }, "slow");
                let page = e.target.innerText;
                let problemId = $('.active-problem')[0].dataset.id;
               
                getSubmissions(problemId, page);
            });
        })
        .fail((error) => {
            console.log(error);
        });
}

function getSubmissions(id, page) {
	let contestId = $('#submit-btn')[0].dataset.contestid;
	$.get(`/Submission/GetProblemSubmissions?problemId=${id}&page=${page}&contestId=${contestId}`)
		.done(response => {
			let tbody = $('#submissions-holder tbody');
			$('#submissions-holder tbody tr').remove();
			for (let submission of response) {
				let tr = generateTr(submission);
				tbody.append(tr);
			}

			$('.page-number > a').removeClass(currentPageClass);
			$($('.page-number > a')[page - 1]).addClass(currentPageClass);

		})
		.fail(error => {
			console.log(error);
		});
}

function generateTr(submission) {
	let tr = $('<tr></tr>');
	let pointsTd = $('<td class="pt-4"></td>');
	let executionInfo = $('<td></td>');
	executionInfo.append(`<span class="d-block">Memory: ${submission.totalMemoryUsed.toFixed(3)} MB</span>`);
	executionInfo.append(`<span class="d-block">Time: ${submission.totalTimeUsed.toFixed(3)} s</span>`);
	let submissionDateTd = $(`<td class="pt-4">${submission.submissionDate}</td>`);
	let detailsBtn = $(`<a href="/Submission/Details?id=${submission.id}" class="btn btn-success" target="_blank">Details</a>`);
	if (!submission.isCompiledSuccessfully) {
		pointsTd.text("Compile time error");
	}
	else {
		for (let test of submission.executedTests) {
			if (test.isCorrect && test.executionResultType === "Success") {
				pointsTd.append('<i class="fas fa-check text-success"></i>');
			}
			else if (test.executionResultType === "RunTimeError") {
				pointsTd.append('<i class="fas fa-bomb text-danger"></i>');
			}
			else if (test.executionResultType === "MemoryLimit") {
				pointsTd.append('<i class="fas fa-memory text-danger"></i>');
			}
			else if (test.executionResultType === "TimeLimit") {
				pointsTd.append('<i class="far fa-clock text-primary"></i>');
			}
			else {
				pointsTd.append('<i class="fas fa-times text-danger"></i>');
			}
		}

		pointsTd.append($(`<span class="ml-3">${submission.actualPoints}/${submission.maxPoints}</span>`));
	}
	tr.append(pointsTd);
	tr.append(executionInfo);
	tr.append(submissionDateTd);
	tr.append($('<td class="pt-3">').append(detailsBtn));
	return tr;
}

$("#previous").on('click', () => {
	$("html, body").animate({ scrollTop: $(document).height() }, "slow");
	let currentPageNumber = $(`.${currentPageClass}`)[0].innerText;
	let lastPage = $(".page-number").length;
	let problemId = $('.active-problem')[0].dataset.id;

	if (currentPageNumber == 1) {
		getSubmissions(problemId, lastPage);
	}
	else {
		getSubmissions(problemId, --currentPageNumber);
	}
});

$("#next").on('click', () => {
	$("html, body").animate({ scrollTop: $(document).height() }, "slow");
	let currentPageNumber = $(`.${currentPageClass}`)[0].innerText;
	let lastPage = $(".page-number").length;
	let problemId = $('.active-problem')[0].dataset.id;

	if (currentPageNumber == lastPage) {
		getSubmissions(problemId, 1);
	}
	else {
		getSubmissions(problemId, ++currentPageNumber);
	}
});

function hideOneOfCodeInputs(submissionType) {
    if (submissionType === zipFileSubmissionType) {
        $(".code-wrapper").hide();
        $("#zip-file").show();
    }
    else {
        $("#zip-file").hide();
        $(".code-wrapper").show();
    }
}

function hideLoader() {
    $("#submissions-holder table tbody tr:first").hide();
}