jQuery(function ($) {

	//dashboard
	//var data;

	$.ajax({
		url: '/Admin/Home/GetChartData',

		type: "POST",
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (response) {
			if (response != null) {

				
				var data = JSON.parse(response);

				var placeholder = $('#piechart-placeholder').css({ 'width': '90%', 'min-height': '150px' });

				//console.log(data);
				//$.each(data, function (i, item) {
				//	$("#ElectionId").append($('<option></option>').attr("value", item.ElectionId).text(item.Name));
				//});

				function drawPieChart(placeholder, data, position) {
					$.plot(placeholder, data, {
						series: {
							pie: {
								show: true,
								tilt: 0.8,
								highlight: {
									opacity: 0.25
								},
								stroke: {
									color: '#fff',
									width: 2
								},
								startAngle: 2
							}
						},
						legend: {
							show: true,
							position: position || "ne",
							labelBoxBorderColor: null,
							margin: [-30, 15]
						}
						,
						grid: {
							hoverable: true,
							clickable: true
						}
					})
				}
				drawPieChart(placeholder, data);

				/**
						 we saved the drawing function and the data to redraw with different position later when switching to RTL mode dynamically
						 so that's not needed actually.
						 */
				placeholder.data('chart', data);
				placeholder.data('draw', drawPieChart);


				//pie chart tooltip example
				var $tooltip = $("<div class='tooltip top in'><div class='tooltip-inner'></div></div>").hide().appendTo('body');
				var previousPoint = null;

				placeholder.on('plothover', function (event, pos, item) {
					if (item) {
						if (previousPoint != item.seriesIndex) {
							previousPoint = item.seriesIndex;
							var tip = item.series['label'] + " : " + item.series['percent'] + '%';
							$tooltip.show().children(0).text(tip);
						}
						$tooltip.css({ top: pos.pageY + 10, left: pos.pageX + 10 });
					} else {
						$tooltip.hide();
						previousPoint = null;
					}

				});
			}
		},
		error: function (e) {
			console.log(e.responseText);
		}
	});
	
	

	$validation = true;

	$('.select2').css('width', '300px').select2({ allowClear: true })
		.on('change', function () {
			$(this).closest('form').validate().element($(this));
		});


	$('#electralpositionform').validate({
		errorElement: 'div',
		errorClass: 'help-block',
		focusInvalid: false,
		ignore: "",
		rules: {
			Name: {
				required: true
			},
			ElectionId: {
				required: true
			},
			Sequence: {
				required: true,
				digits: true
            }
		},

		messages: {

			Name: "Please specify the electral position name",
			ElectionId: "Please select the election",
			Sequence: {
				digits: "Sequence can only be an integer value",
				required: "Please specify the squence as to appear on the voter's window"
			}
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
				url: '/Positions/CreatePosition',
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
				url: '/Positions/UpdatePosition',
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

						if (data.Status == "000") {
							$.gritter.add({
								title: 'Delete Notification',
								text: data.Message,
								class_name: 'gritter-info gritter-center'
							});
						} else {
							$.gritter.add({
								title: 'Delete Notification',
								text: data.Message,
								class_name: 'gritter-error gritter-center'
							});
						}

						//redirect after 2 seconds
						window.setTimeout(function () {
							window.location.href = '../Index';
						}, 2000);
						
					}
				},
				error: function (e) {
					console.log(e.responseText);
				}
			});
		}

		event.preventDefault();
	});
	$("#positions-table").on("click", ".deleteposition", function (e) {
		e.preventDefault();

		var docno = $(this).attr('data-docno');
		var recordname = $(this).attr('data-recordname');


		bootbox.confirm({
			title: "<i class='fa fa-trash'></i> Delete?",
			message: "Do you wish to delete the position " + recordname + "?",
			buttons: {
				confirm: {
					label: 'Yes',
					className: 'btn-success'
				},
				cancel: {
					label: 'No',
					className: 'btn-danger'
				}
			},
			callback: function (result) {

				if (result == true) {

					jQuery.ajax({
						url: '/Positions/Delete',
						type: "POST",
						data: '{DocumentNo:"' + docno + '" }',
						dataType: "json",
						contentType: "application/json; charset=utf-8",
						success: function (response) {

							if (response != null) {
								//console.log(JSON.stringify(response)); //it comes out to be string 

								//we need to parse it to JSON
								var data = $.parseJSON(response);


								if (data.Status == "000") {
									$.gritter.add({
										title: 'Delete Notification',
										text: data.Message,
										class_name: 'gritter-info gritter-center'
									});
								} else {
									$.gritter.add({
										title: 'Delete Notification',
										text: data.Message,
										class_name: 'gritter-error gritter-center'
									});
								}

								//reload after 2 seconds
								window.setTimeout(function () {
									location.reload(true);
								}, 2000);
							}
						}
					});
				}
			}
		});
	});

	//electral candidates

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

		//	var urllink = $('#electralcandidateform').attr('action');

			jQuery.ajax({
				url: '/Candidates/CreateCandidate',
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
	$("#UpdateElectralCandidate").click(function (event) {

		if ($('#electralcandidateform').valid()) {
			//Serialize the form datas.  
			var valdata = $("#electralcandidateform").serialize();
			//to get alert popup  

			//	var urllink = $('#electralcandidateform').attr('action');

			jQuery.ajax({
				url: '/Candidates/UpdateCandidate',
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

	$("#candidates-table").on("click", ".deletecandidate", function (e) {
		e.preventDefault();

		var docno = $(this).attr('data-docno');
		var recordname = $(this).attr('data-recordname');
		

		bootbox.confirm({
			title: "<i class='fa fa-trash'></i> Delete?",
			message: "Do you wish to delete " + recordname + " as a candidate?",
			buttons: {
				confirm: {
					label: 'Yes',
					className: 'btn-success'
				},
				cancel: {
					label: 'No',
					className: 'btn-danger'
				}
			},
			callback: function (result) {

				if (result == true) {

					jQuery.ajax({
						url: '/Candidates/Delete',
						type: "POST",
						data: '{DocumentNo:"' + docno + '" }',
						dataType: "json",
						contentType: "application/json; charset=utf-8",
						success: function (response) {

							if (response != null) {
								//console.log(JSON.stringify(response)); //it comes out to be string 

								//we need to parse it to JSON
								var data = $.parseJSON(response);


								if (data.Status == "000") {
									$.gritter.add({
										title: 'Delete Notification',
										text: data.Message,
										class_name: 'gritter-info gritter-center'
									});
								} else {
									$.gritter.add({
										title: 'Delete Notification',
										text: data.Message,
										class_name: 'gritter-error gritter-center'
									});
								}
							}
						}
					});
				}
			}
		});
	});

	//separate pages

	//list students
	//$.ajax({
	//	url: '/Admin/Students/ListStudents',
	//	type: "POST",
	//	dataType: "json",
	//	contentType: "application/json; charset=utf-8",
	//	success: function (response) {
	//		if (response != null) {

	//			var data = $.parseJSON(response);

	//			$.each(data, function (i, item) {
	//				$("#StudentId").append($('<option></option>').attr("value", item.StudentId).text(item.Name));
	//			});

	//		}
	//	},
	//	error: function (e) {
	//		console.log(e.responseText);
	//	}
	//});
	//list electral positions
	//$.ajax({
	//	url: '/Admin/Positions/ListPositions',
	//	type: "POST",
	//	dataType: "json",
	//	contentType: "application/json; charset=utf-8",
	//	success: function (response) {
	//		if (response != null) {

	//			var data = $.parseJSON(response);

	//			$.each(data, function (i, item) {
	//				$("#PositionId").append($('<option></option>').attr("value", item.PositionId).text(item.Name));
	//			});

	//		}
	//	},
	//	error: function (e) {
	//		console.log(e.responseText);
	//	}
	//});

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
				url: '/Elections/CreateElection',
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

	$("#UpdateElection").click(function (event) {

		if ($('#electionform').valid()) {
			//Serialize the form datas.  
			var valdata = $("#electionform").serialize();
			//to get alert popup  	

			jQuery.ajax({
				url: '/Elections/UpdateElection',
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


	$("#elections-table").on("click", ".deleteelection", function (e) {
		e.preventDefault();

		var docno = $(this).attr('data-docno');
		var recordname = $(this).attr('data-recordname');


		bootbox.confirm({
			title: "<i class='fa fa-trash'></i> Delete?",
			message: "Do you wish to delete the election " + recordname + "?",
			buttons: {
				confirm: {
					label: 'Yes',
					className: 'btn-success'
				},
				cancel: {
					label: 'No',
					className: 'btn-danger'
				}
			},
			callback: function (result) {

				if (result == true) {

					jQuery.ajax({
						url: '/Elections/Delete',
						type: "POST",
						data: '{DocumentNo:"' + docno + '" }',
						dataType: "json",
						contentType: "application/json; charset=utf-8",
						success: function (response) {

							if (response != null) {
								//console.log(JSON.stringify(response)); //it comes out to be string 

								//we need to parse it to JSON
								var data = $.parseJSON(response);


								if (data.Status == "000") {
									$.gritter.add({
										title: 'Delete Notification',
										text: data.Message,
										class_name: 'gritter-info gritter-center'
									});
								} else {
									$.gritter.add({
										title: 'Delete Notification',
										text: data.Message,
										class_name: 'gritter-error gritter-center'
									});
								}
							}
						}
					});
				}
			}
		});
	});
	$("#elections-table").on("click", ".openelection", function (e) {
		e.preventDefault();

		var docno = $(this).attr('data-docno');

		bootbox.confirm({
			title: "<i class='fa fa-paper-plane'></i> Open election for voting?",
			message: "Do you wish to open election " + docno + " for voting by students?",
			buttons: {
				confirm: {
					label: 'Yes',
					className: 'btn-success'
				},
				cancel: {
					label: 'No',
					className: 'btn-danger'
				}
			},
			callback: function (result) {

				if (result == true) {

					jQuery.ajax({
						url: '/Elections/Open',
						type: "POST",
						data: '{ElectionId:"' + docno + '" }',
						dataType: "json",
						contentType: "application/json; charset=utf-8",
						success: function (response) {

							if (response != null) {
								//console.log(JSON.stringify(response)); //it comes out to be string 

								//we need to parse it to JSON
								var data = $.parseJSON(response);

								if (data.Status == "000") {
									$.gritter.add({
										title: 'Approval Notification',
										text: data.Message,
										class_name: 'gritter-info gritter-center'
									});
								} else {
									$.gritter.add({
										title: 'Approval Notification',
										text: data.Message,
										class_name: 'gritter-error gritter-center'
									});
								}
							}
						}
					});
				}
			}
		});
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
				Phone: 'required'
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
			var valdata = $("#studentform").serialize();
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
	$("#UpdateStudent").click(function (event) {

		if ($('#studentform').valid()) {
			//Serialize the form datas.  
			var valdata = $("#studentform").serialize();
			//to get alert popup  	

			jQuery.ajax({
				url: '/Admin/Students/UpdateStudent',
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
	$("#students-table").on("click", ".deletestudent", function (e) {
		e.preventDefault();

		var docno = $(this).attr('data-docno');
		var recordname = $(this).attr('data-recordname');


		bootbox.confirm({
			title: "<i class='fa fa-trash'></i> Delete?",
			message: "Do you wish to delete " + recordname + " from the system?",
			buttons: {
				confirm: {
					label: 'Yes',
					className: 'btn-success'
				},
				cancel: {
					label: 'No',
					className: 'btn-danger'
				}
			},
			callback: function (result) {

				if (result == true) {

					jQuery.ajax({
						url: '/Students/Delete',
						type: "POST",
						data: '{DocumentNo:"' + docno + '" }',
						dataType: "json",
						contentType: "application/json; charset=utf-8",
						success: function (response) {

							if (response != null) {
								//console.log(JSON.stringify(response)); //it comes out to be string 

								//we need to parse it to JSON
								var data = $.parseJSON(response);


								if (data.Status == "000") {
									$.gritter.add({
										title: 'Delete Notification',
										text: data.Message,
										class_name: 'gritter-info gritter-center'
									});
								} else {
									$.gritter.add({
										title: 'Delete Notification',
										text: data.Message,
										class_name: 'gritter-error gritter-center'
									});
								}
							}
						}
					});
				}
			}
		});
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