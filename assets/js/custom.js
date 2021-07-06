jQuery(function ($) {

	$validation = true;

	$('.select2').css('width', '200px').select2({ allowClear: true })
		.on('change', function () {
			$(this).closest('form').validate().element($(this));
		});

	//electral position
	

	$.ajax({
		url: '/Admin/Elections/ListElections', 

		type: "POST",
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (response) {
			if (response != null) {

				var data = $.parseJSON(response);

				$.each(data, function (i, item) {
					$("#ElectionId").append($('<option></option>').attr("value", item.ElectionId).text(item.Name));
				});

			}
		},
		error: function (e) {
			console.log(e.responseText);
		}
	});

	$('#electralpositionform').validate({
		errorElement: 'div',
		errorClass: 'help-block',
		focusInvalid: false,
		ignore: "",
		rules: {
			name: {
				required: true
			},
			ElectionId: {
				required: true
			},
			Sequence: {
				required: true
            }
		},

		messages: {

			name: "Please specify the electral position name",
			ElectionId: "Please select the election",
			Sequence: "Please select position on the voters window"
		},


		highlight: function (e) {
			$(e).closest('.form-group').removeClass('has-info').addClass('has-error');
		},

		success: function (e) {
			$(e).closest('.form-group').removeClass('has-error');//.addClass('has-info');
			$(e).remove();
		},

		errorPlacement: function (error, element) {
			if (element.is('input[type=checkbox]') || element.is('input[type=radio]')) {
				var controls = element.closest('div[class*="col-"]');
				if (controls.find(':checkbox,:radio').length > 1) controls.append(error);
				else error.insertAfter(element.nextAll('.lbl:eq(0)').eq(0));
			}
			else if (element.is('.select2')) {
				error.insertAfter(element.siblings('[class*="select2-container"]:eq(0)'));
			}
			else if (element.is('.chosen-select')) {
				error.insertAfter(element.siblings('[class*="chosen-container"]:eq(0)'));
			}
			else error.insertAfter(element.parent());
		},

		submitHandler: function (form) {
		},
		invalidHandler: function (form) {
		}
	});

	$("#SaveElectralposition").click(function (event) {

		//var formAction = $("#electralpositionform").attr('action');

		//console.log(formAction);


		if ($('#electralpositionform').valid())
		{
			//Serialize the form datas.  
			var valdata = $("#electralpositionform").serialize();
			//to get alert popup  	

			jQuery.ajax({
				url: './CreatePosition',
				type: "POST",
				data: valdata,
				dataType: "json",
				contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
				success: function (response) {
					if (response != null) {
						//console.log(JSON.stringify(response)); //it comes out to be string 

						//we need to parse it to JSON
						var data = $.parseJSON(response);

						//console.log(data.Message);
						bootbox.dialog({
							message: data.Message,
							buttons: {
								"success": {
									"label": "OK",
									"className": "btn-sm btn-primary"
								}
							}
						});
					}
				},
				error: function (e) {
					console.log(e.responseText);
				}
			});
        }		

		event.preventDefault();
	});

	$("#UpdateElectralposition").click(function (event) {

		var formAction = $("#electralpositionform").attr('action');

		console.log(formAction);

		if ($('#electralpositionform').valid()) {

			var valdata = $("#electralpositionform").serialize();

			jQuery.ajax({
				url: '../UpdatePosition',
				type: "POST",
				data: valdata,
				dataType: "json",
				contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
				success: function (response) {
					if (response != null) {
						//console.log(JSON.stringify(response)); //it comes out to be string 

						//we need to parse it to JSON
						var data = $.parseJSON(response);

						//console.log(data.Message);

						bootbox.dialog({
							message: data.Message,
							buttons: {
								"success": {
									"label": "OK",
									"className": "btn-sm btn-primary"
								}								
							}
						});

						window.location.href = '../Index';
					}
				},
				error: function (e) {
					console.log(e.responseText);
				}
			});
		}

		event.preventDefault();
	});

	//electral candidate

	$('#electralcandidateform').validate({
		errorElement: 'div',
		errorClass: 'help-block',
		focusInvalid: false,
		ignore: "",
		rules: {			
			PositionId: {
				required: true
			},
			StudentId: {
				required: true
			},
			ElectionId: {
				required: true
			}
		},

		messages: {			
			StudentId: "Please choose student",
			PositionId: "Please choose position",
			ElectionId: "Please choose the election"
		},


		highlight: function (e) {
			$(e).closest('.form-group').removeClass('has-info').addClass('has-error');
		},

		success: function (e) {
			$(e).closest('.form-group').removeClass('has-error');//.addClass('has-info');
			$(e).remove();
		},

		errorPlacement: function (error, element) {
			if (element.is('input[type=checkbox]') || element.is('input[type=radio]')) {
				var controls = element.closest('div[class*="col-"]');
				if (controls.find(':checkbox,:radio').length > 1) controls.append(error);
				else error.insertAfter(element.nextAll('.lbl:eq(0)').eq(0));
			}
			else if (element.is('.select2')) {
				error.insertAfter(element.siblings('[class*="select2-container"]:eq(0)'));
			}
			else if (element.is('.chosen-select')) {
				error.insertAfter(element.siblings('[class*="chosen-container"]:eq(0)'));
			}
			else error.insertAfter(element.parent());
		},

		submitHandler: function (form) {
		},
		invalidHandler: function (form) {
		}
	});

	$("#SaveElectralCandidate").click(function (event) {

		if ($('#electralcandidateform').valid()) {
			//Serialize the form datas.  
			var valdata = $("#electralcandidateform").serialize();
			//to get alert popup  	

			jQuery.ajax({
				url: './CreateCandidate',
				type: "POST",
				data: valdata,
				dataType: "json",
				contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
				success: function (response) {
					if (response != null) {
						//console.log(JSON.stringify(response)); //it comes out to be string 

						//we need to parse it to JSON
						var data = $.parseJSON(response);

						//console.log(data.Message);
						bootbox.dialog({
							message: data.Message,
							buttons: {
								"success": {
									"label": "OK",
									"className": "btn-sm btn-primary"
								}
							}
						});
					}
				},
				error: function (e) {
					console.log(e.responseText);
				}
			});
		}

		event.preventDefault();
	});

	//separate pages

	//list students
	$.ajax({
		url: '/Admin/Students/ListStudents',
		type: "POST",
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (response) {
			if (response != null) {

				var data = $.parseJSON(response);

				$.each(data, function (i, item) {
					$("#StudentId").append($('<option></option>').attr("value", item.StudentId).text(item.Name));
				});

			}
		},
		error: function (e) {
			console.log(e.responseText);
		}
	});
	//list electral positions
	$.ajax({
		url: '/Admin/Positions/ListPositions',
		type: "POST",
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (response) {
			if (response != null) {

				var data = $.parseJSON(response);

				$.each(data, function (i, item) {
					$("#PositionId").append($('<option></option>').attr("value", item.PositionId).text(item.Name));
				});

			}
		},
		error: function (e) {
			console.log(e.responseText);
		}
	});

	/// Elections

	if (!ace.vars['old_ie']) $('#StartDate, #EndDate').datetimepicker({
		//format: 'MM/DD/YYYY h:mm:ss A',//use this option to display seconds
		icons: {
			time: 'fa fa-clock-o',
			date: 'fa fa-calendar',
			up: 'fa fa-chevron-up',
			down: 'fa fa-chevron-down',
			previous: 'fa fa-chevron-left',
			next: 'fa fa-chevron-right',
			today: 'fa fa-arrows ',
			clear: 'fa fa-trash',
			close: 'fa fa-times'
		}
	}).next().on(ace.click_event, function () {
		$(this).prev().focus();
	});


	$('#electionform').validate({
		errorElement: 'div',
		errorClass: 'help-block',
		focusInvalid: false,
		ignore: "",
		rules: {
			name: {
				required: true
			},
			StartDate: {
				required: true
			},
			EndDate: {
				required: true
			}
		},

		messages: {

			name: "Please specify the electral position name",
			StartDate: "Please specify the election start date and time",
			EndDate: "Please specify the election end date and time"
		},


		highlight: function (e) {
			$(e).closest('.form-group').removeClass('has-info').addClass('has-error');
		},

		success: function (e) {
			$(e).closest('.form-group').removeClass('has-error');//.addClass('has-info');
			$(e).remove();
		},

		errorPlacement: function (error, element) {
			if (element.is('input[type=checkbox]') || element.is('input[type=radio]')) {
				var controls = element.closest('div[class*="col-"]');
				if (controls.find(':checkbox,:radio').length > 1) controls.append(error);
				else error.insertAfter(element.nextAll('.lbl:eq(0)').eq(0));
			}
			else if (element.is('.select2')) {
				error.insertAfter(element.siblings('[class*="select2-container"]:eq(0)'));
			}
			else if (element.is('.chosen-select')) {
				error.insertAfter(element.siblings('[class*="chosen-container"]:eq(0)'));
			}
			else error.insertAfter(element.parent());
		},

		submitHandler: function (form) {
		},
		invalidHandler: function (form) {
		}
	});

	$("#SaveElection").click(function (event) {

		if ($('#electionform').valid()) {
			//Serialize the form datas.  
			var valdata = $("#electionform").serialize();
			//to get alert popup  	

			jQuery.ajax({
				url: './CreateElection',
				type: "POST",
				data: valdata,
				dataType: "json",
				contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
				success: function (response) {
					if (response != null) {
						//console.log(JSON.stringify(response)); //it comes out to be string 

						//we need to parse it to JSON
						var data = $.parseJSON(response);

						//console.log(data.Message);
						bootbox.dialog({
							message: data.Message,
							buttons: {
								"success": {
									"label": "OK",
									"className": "btn-sm btn-primary"
								}
							}
						});
					}
				},
				error: function (e) {
					console.log(e.responseText);
				}
			});
		}

		event.preventDefault();
	});

	///students


	$.mask.definitions['~'] = '[+-]';
	$('#Phone').mask('+254 999-999999');

	jQuery.validator.addMethod("Phone", function (value, element) {
		return this.optional(element) || /^\+\d{3}\ \d{3}\-\d{6}( x\d{1,6})?$/.test(value);
	}, "Enter a valid phone number.");

	$('#studentform').validate({
		errorElement: 'div',
		errorClass: 'help-block',
		focusInvalid: false,
		ignore: "",
		rules: {
			Email: {
				required: true,
				email: true
			},
			Password: {
				required: true,
				minlength: 5
			},
			ConfirmPassword: {
				required: true,
				minlength: 5,
				equalTo: "#Password"
			},
			Phone: {
				required: true,
				phone: 'required'
			},
			FirstName: {
				required: true
			},
			LastName: {
				required: true
			},
			Name: {
				required: true
			},
			Faculty: {
				required: true
			},
			Gender: {
				required: true,
			},
			YearOfStudy: {
				required: true,
			}
		},

		messages: {
			Email: {
				required: "Please provide a valid email.",
				email: "Please provide a valid email."
			},
			Password: {
				required: "Please specify a password.",
				minlength: "Please specify a secure password."
			},
			Phone: "Please provide a valid number",
			Faculty: "Please choose faculty",
			YearOfStudy: "Please choose year of study"
		},


		highlight: function (e) {
			$(e).closest('.form-group').removeClass('has-info').addClass('has-error');
		},

		success: function (e) {
			$(e).closest('.form-group').removeClass('has-error');//.addClass('has-info');
			$(e).remove();
		},

		errorPlacement: function (error, element) {
			if (element.is('input[type=checkbox]') || element.is('input[type=radio]')) {
				var controls = element.closest('div[class*="col-"]');
				if (controls.find(':checkbox,:radio').length > 1) controls.append(error);
				else error.insertAfter(element.nextAll('.lbl:eq(0)').eq(0));
			}
			else if (element.is('.select2')) {
				error.insertAfter(element.siblings('[class*="select2-container"]:eq(0)'));
			}
			else if (element.is('.chosen-select')) {
				error.insertAfter(element.siblings('[class*="chosen-container"]:eq(0)'));
			}
			else error.insertAfter(element.parent());
		},

		submitHandler: function (form) {
		},
		invalidHandler: function (form) {
		}
	});

	$("#SaveStudent").click(function (event) {

		if ($('#studentform').valid()) {
			//Serialize the form datas.  
			var valdata = $("#electralcandidateform").serialize();
			//to get alert popup  	

			jQuery.ajax({
				url: '/Admin/Students/CreateStudent',
				type: "POST",
				data: valdata,
				dataType: "json",
				contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
				success: function (response) {
					if (response != null) {
						//console.log(JSON.stringify(response)); //it comes out to be string 

						//we need to parse it to JSON
						var data = $.parseJSON(response);

						//console.log(data.Message);
						bootbox.dialog({
							message: data.Message,
							buttons: {
								"success": {
									"label": "OK",
									"className": "btn-sm btn-primary"
								}
							}
						});
					}
				},
				error: function (e) {
					console.log(e.responseText);
				}
			});
		}

		event.preventDefault();
	});


	//cast vote

	$('#fuelux-wizard-container')
		.ace_wizard({
			//step: 2 //optional argument. wizard will jump to step "2" at first
			//buttons: '.wizard-actions:eq(0)'
		})
		.on('actionclicked.fu.wizard', function (e, info) {
			//if (info.step == 1 && $validation) {
			//	if (!$('#validation-form').valid()) e.preventDefault();
			//}
		})
		//.on('changed.fu.wizard', function() {
		//})
		.on('finished.fu.wizard', function (e) {

			//fetch here

			var valdata = $("#ballot-form").serializeArray();
			//to get alert popup  	
			
			jQuery.ajax({
				url: '../CastBallot',
				type: "POST",
				contentType: "application/json",
				data: JSON.stringify({ formVars: valdata }),
				dataType: "json",
				success: function (response) {
					if (response != null) {
						//console.log(JSON.stringify(response)); //it comes out to be string 

						//we need to parse it to JSON
						var data = $.parseJSON(response);

						//console.log(data.Message);
						bootbox.dialog({
							message: data.Message,
							buttons: {
								"success": {
									"label": "OK",
									"className": "btn-sm btn-primary"
								}
							}
						});
					}
				},
				error: function (e) {
					console.log(e.statusCode);
				}
			});
		}).on('stepclick.fu.wizard', function (e) {
			//e.preventDefault();//this will prevent clicking and selecting steps
		});
	

})