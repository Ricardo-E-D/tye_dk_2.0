/// assign striping css classes to rows of tables with class name "stripe"
function jquery_stripeTables(options) {
	$('table.stripe').each(
		function () {
			var o = $(this); // table
			var evenRows = o.find(' > tbody >tr');
			var oddRows = o.find(' > tbody >tr');
			if (typeof (options) != "undefined") {
				if (typeof (options.onlyVisible) != "undefined" && options.onlyVisible) {
					evenRows = evenRows.filter(':visible');
					oddRows = evenRows.filter(':visible');
				}
			}

			evenRows = evenRows.filter(':even');
			oddRows = oddRows.filter(':odd');

			if (o.hasClass('stripeSkipFirst')) {
				//o.find(' > tbody >tr').filter(':even').not(':first').removeClass('odd').addClass('even');
				evenRows = evenRows.not(':first');
			} else {
				//o.find(' > tbody >tr').filter(':even').removeClass('odd').addClass('even');
			}
			//o.find('>tbody >tr ').filter(':odd').removeClass('even').addClass('odd');
			evenRows.removeClass('odd').addClass('even');
			oddRows.removeClass('even').addClass('odd');
		}
	);
}

/// finds table rows with "clickurl" attributes and assign a click event that liks to clickurl value
function jquery_assignClickUrl() {
	$(document)
		.find('table tr[clickurl]')
		.addClass('link')
		.on('click', 'td', function () {
			if (!$(this).hasClass('delete')) {
				if (typeof($(this).attr('clickurltarget')) != "undefined" && $(this).attr('clickurltarget') != "") {
					window.open($(this).parent().attr('clickurl'), $(this).attr('clickurltarget'));
				}
				else
					window.location.href = $(this).parent().attr('clickurl');
			}
		});
}

// loop through a table and fix the first column size making it as small as possible
function minimizeFirstColumn(table) {
	var o = (typeof (table) == "string" ? $('#' + table) : $(table));
	o.find('tr td:first-child').each(function () {
		var cell = $(this);
		cell.width(cell.textWidth() + 10);
	});
}

// clickOnEnter performs a 'click' event on the target object when the enter key is pressed in the source object.
// for instance, in a aspx-page where a "search" button has default focus but you want the "login" button to be activated
// then 'enter' is pressed in the "username" or "password" textboxes....this is just what you need
(function ($) {
	var methods = {
		init: function (object) {

			var instance = this;
			var objects;
			
			if (arguments.length > 0 && (typeof (arguments[0]) == "string")) {
				objects = $(this);
				objects.attr('clickonenter', arguments[0]);
			} else {
				objects = (this.attr('clickonenter') != undefined ? this : this.find("*[clickonenter]"));
			}
			
			return objects.each(function (intIndex, node) {
				var obj = $(node);
				if (obj.attr("clickonenter") !== undefined) {
					var clickTarget = $('#' + obj.attr("clickonenter"));
					if (clickTarget.length > 0) {
						obj.bind('keypress.clickOnEnter', function (event) {
							return methods.onEnterKey.apply(
									this, new Array(
										event, clickTarget
									)
								);
						});
					}
				}
			});
		},
		onEnterKey: function (evt, target) {
			
			if (evt.keyCode == 13) { // enter
				$(target).click();
				return false;
			} else {
				return true;
			}
		}
	}
	$.fn.clickOnEnter = function (options) {
		return methods.init.apply(this, arguments); // Array.prototype.slice.call(arguments, 1));
	};
})(jQuery);


function addCheckEvent() {
	$(document).on('mouseover', 'input[type=checkbox]', function(evt) {
		if(evt.ctrlKey)
			this.checked = !this.checked;
	});
}

function sortTable(obj, expr) {
	
	//var $sort = $(obj);
	var $table = $(obj), 
		 expression = (typeof (expr) == 'undefined' ? 'td:first-child' : expr),
		 $rows = $('tbody > tr', $table);

	$rows.sort(function (a, b) {
		var keyA = $(expression, a).text();
		var keyB = $(expression, b).text();
		return (keyA > keyB) ? 1 : 0;
		/*if ($($sort).hasClass('asc')) {
			return (keyA > keyB) ? 1 : 0;
		} else {
			return (keyA > keyB) ? 1 : 0;
		}*/
	});
	$.each($rows, function (index, row) {
		$table.append(row);
	});
}

// <double with mobi>
function calendarBelowTextbox(input, calendar) {
	// don't know why I have to parseInt....but I do (?)
	var top = parseInt(input.position().top, 10) - 50;
	var docBottom = $(window).innerHeight() + $(window).scrollTop();
	$(calendar).css(
		{
			top: top + "px",
			left: (input.offset().left < 20 ? 1 : parseInt(input.position().left, 10)) + "px"
		});
	if (typeof ($.center) != "undefined")
		$(calendar).center();
	input.blur();
}
// </double with mobi>