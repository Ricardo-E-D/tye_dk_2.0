// jquery.monoTableFilter.js
// (c) by monosolutions, 2013.07.10, v1


(function ($) {
	var monotablefilter = this,
		options = {
			table: null,
			input: null,
			cookie_name: null
		},
		methods = {
			// initialization
			init: function () {
				$(options.input).on('keyup', function () {
					$.doTimeout('monoTableFilter', options.delay, function () {
						methods.filter.apply(this);
					});
				});
				if (options.elementClearFilter != null) {
					$(options.elementClearFilter).on('click', function () {
						methods.clearFilter.apply(this);
					});
				}
				methods.getCookieValue.apply();
				methods.filter.apply();
				if (options.autoSetFocusToInput) {
					options.input.focus().select();
				}
				return monotablefilter;
			}, // init
			filter: function () {
				if (typeof (options.input) == "undefined" || options.input.length == 0) {
					return;
				}
				if (options.eventBeforeFilter != null) {
					options.eventBeforeFilter.apply(this);
				}
				if (options.input.val().length == 0) {
					options.table.find("tbody tr").show();
				} else {
					options.table.find("tbody tr").hide();
					var data = options.input.val().split(" ");
					var jo = options.table.find("tbody tr");
					$.each(data, function (i, v) {
						//Use the new containsIgnoreCase function instead
						jo = jo.filter("*:containsIgnoreCase('" + v + "')");
					});
					jo.show();
				}
				methods.setCookieValue.apply(this);
				if (options.eventAfterFilter != null) {
					options.eventAfterFilter.apply(this);
				}
			},
			clearFilter: function () {
				options.input.val('');
				methods.filter.apply(this);
				methods.setCookieValue.apply(this);
			},
			getCookieValue: function () {
				if (options.cookieName != null) {
					options.input.val($.cookie("monoTableFilter" + options.cookieName));
				}
			},
			setCookieValue: function () {
				if (options.cookieName != null) {
					$.cookie("monoTableFilter" + options.cookieName, options.input.val());
				}
			}
		};

	$.fn.monoTableFilter = function (inputElement, opts) {
		if (typeof ($.doTimeout) == "undefined") {
			alert("monoTableFilter requires jquery.doTimeout");
			return;
		}

		$.expr[':'].containsIgnoreCase = function (n, i, m) {
			return jQuery(n).text().toUpperCase().indexOf(m[3].toUpperCase()) >= 0;
		};
		var defaults = {
			cookie_name: null,
			table: $(this),
			input: $(inputElement),
			delay: 500,
			elementClearFilter: null,
			cookieName: null,
			eventBeforeFilter: null,
			eventAfterFilter: null,
			autoSetFocusToInput: false
		};
		options = $.extend(defaults, opts);

		if (options.cookieName != null && typeof ($.cookie) == "undefined") {
			alert("jquery.cookie.js is required when using cookies");
			return;
		}

		return methods.init.apply(this);
		//return this;
	};

})(jQuery);