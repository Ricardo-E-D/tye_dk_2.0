// jquery.monoTabs.js
// by monosolutions, 2012.03.14, v1
// by monosolutions, 2012.07.02, v1.1


(function ($) {
	var monotabs = this,
		options = {
			tabCount: 0,
			container: null
		},
		methods = {
			// adds a tab to DOM. 'insertIndex' argument is optional
			addTab: function (tabText, tabPanel, insertIndex) {
				var element = null,
					 tab = $('<li>' + tabText + '</li>');
				switch (typeof (tabPanel)) {
					case "object":
						element = tabPanel;
						break;
					case "string":
						if (document.getElementById(tabPanel) === null) {
							element = $('<div class="tabPanel"></div>');
							element.html(tabPanel);
						} else {  // assume html content
							element = $(document.getElementById(tabPanel));
						}
						break;
				}
				if (element === null) {
					return;
				}
				element.addClass("tabPanel");


				// insert elements
				if (!isNaN(parseInt(insertIndex, 10))) {
					var ii = Math.max(parseInt(insertIndex, 10), 0);
					ii = Math.min(ii, options.tabCount);
					if (ii === options.tabCount) { // placing last
						options.container.find('.innerTabs ul li:nth-child(' + ii + ')').after(tab);
						options.container.find('.innerContainer .tabPanel:nth-child(' + ii + ')').after(element);
					} else { // not last
						ii = ii + 1;
						options.container.find('.innerTabs ul li:nth-child(' + ii + ')').before(tab);
						options.container.find('.innerContainer .tabPanel:nth-child(' + ii + ')').before(element);
					}
				} else {
					options.container.find('.innerTabs ul').append(tab);
					options.container.find('.innerContainer').append(element);
				}

				methods.init.apply(options.container);
			}, // addTab
			// removes a tab and elements from DOM
			destroyTab: function (positionIndex) {
				var index = parseInt(positionIndex, 10);
				if (!isNaN(index) && index >= 0 && index <= options.tabCount) { 
					var element = options.container.find('.tabPanel:nth-child(' + (index + 1) + ')');
					var html = methods.removeTab.apply(this, [index]).html();
					element.remove();
					return html;
				}
			},
			// hides the tab with index value passed. If not index value passed the currently active tab is hidden.
			hideTab: function (positionIndex) {
				var index = parseInt(positionIndex, 10);
				if (!isNaN(index) && index >= 0 && index <= options.tabCount) {
					options.container.find('.innerTabs ul li:nth-child(' + (index + 1) + ')').hide();
					options.container.find('.innerContainer .tabPanel:nth-child(' + (index + 1) + ')').hide();
				} else if (typeof (index) === "undefined") {
					options.container.find('.innerTabs ul li:nth-child(' + (options.activeTabIndex + 1) + ')').hide();
					options.container.find('.innerContainer .tabPanel:nth-child(' + (options.activeTabIndex + 1) + ')').hide();
				}
				methods.init.apply(this);
			},
			// initialization
			init: function () {
				if (options.container == null) {
					options.container = $(this);
				}

				options.tabCount = parseInt(options.container.find('.innerContainer div.tabPanel').length, 10);
				if (options.activeTabIndex < 0) {
					options.activeTabIndex = 0;
				}
				if (options.activeTabIndex > options.tabCount - 1) {
					options.activeTabIndex = options.tabCount - 1;
				}

				this.each(function () {

					// hide all tabs
					options.container.find('.innerContainer div.tabPanel').hide();
					// show first tab
					options.container.find('.innerContainer div.tabPanel:first').show();

					var blnCookieValueApplied = false;
					// if cookie name specified
					if (options.cookie_name !== null) {
						// bind click event to save active tab index to cookie
						options.container.find('.innerTabs li').bind('click', function (event) {
							$.cookie(options.cookie_name, $(this).parent().find('li').index($(this)));
						});
						// attempt read from cookie
						var cookievalue = parseInt($.cookie(options.cookie_name), 10);
						if (cookievalue > -1 && cookievalue < options.tabCount) {
							methods.switchTab.apply(this, [cookievalue]);
							blnCookieValueApplied = true;
						}
					} // if cookie_name != null

					if (!blnCookieValueApplied && options.activeTabIndex > -1) {
						methods.switchTab.apply(this, [options.activeTabIndex]);
					}

					// for each tab
					options.container.find('.innerTabs li').each(function (index, t) {
						// bind function to switch tab
						$(this).bind('click', function (t, container) {
							methods.switchTab.apply(this, [index]);
						});
					});
				}); // this.each
				return monotabs;
			}, // init
			// moves a tab from one location to another
			moveTab: function (indexFrom, indexTo) {
				if (indexFrom === indexTo)
					return;

				if (indexFrom >= 0 && indexFrom <= options.tabCount && indexTo >= 0 && indexTo <= options.tabCount) {
					var elementFrom = options.container.find('.innerTabs li:nth-child(' + (indexFrom + 1) + ')'),
						elementTo = options.container.find('.innerTabs li:nth-child(' + (indexTo + 1) + ')');
					if (indexFrom > indexTo) { // moving backwards
						elementFrom.before(elementTo);
					} else { // moving forward
						elementFrom.after(elementTo);
					}
				}
			},
			// removes a tab but doesn't remove tab panel and contents from DOM
			removeTab: function (positionIndex) {
				var index = parseInt(positionIndex, 10);
				if (!isNaN(index) && index >= 0 && index <= options.tabCount) {
					options.container.find('.innerTabs li:nth-child(' + (index + 1) + ')').remove();
					//methods.init.apply(options.container);
					var html = options.container.find('.tabPanel:nth-child(' + (index + 1) + ')').html();
					//options.container.find('.tabPanel:nth-child(' + (index + 1) + ')').show();
					var element = options.container.find('.tabPanel:nth-child(' + (index + 1) + ')');
					// move tabPanel element out of the container
					options.container.parent().after(element);
					methods.init.apply(options.container);
					methods.switchTab.apply(this, [(index > 0 ? index - 1 : 0)]);
					element.removeClass('tabPanel');
					return element;
				}
			},
			// shows all hidden tabs
			showAll: function () {
				options.container.find('.innerTabs li').show();
			},
			// shows tab
			showTab: function (index) {
				if (index >= 0 && index < options.tabCount) {
					options.container.find('.innerTabs ul li:nth-child(' + (index + 1) + ')').show();
					methods.switchTab.apply(this, [index]);
				}
			},
			// activates a tab
			switchTab: function (index) {
				options.container.find('.innerTabs li').removeClass('active');
				options.container.find('.innerTabs li:nth-child(' + (index + 1) + ')').addClass('active');
				options.container.find('.tabPanel').hide();
				options.container.find('.tabPanel:nth-child(' + (index + 1) + ')').show();
				options.activeTabIndex = index;
			} // switchTab
		};

	this.addTab = function (tabText, tabPanel, insertIndex) { methods.addTab.apply(this, [tabText, tabPanel, insertIndex]); };
	this.destroyTab = function (index) { return methods.destroyTab.apply(this, [index]); }
	this.hideTab = function (index) { methods.hideTab.apply(this, [index]); };
	this.moveTab = function (indexFrom, indexTo) { methods.moveTab.apply(this, [indexFrom, indexTo]); };
	this.removeTab = function (index) { return methods.removeTab.apply(this, [index]); };
	this.selectTab = function (index) { methods.switchTab(index); };
	this.showAll = function () { methods.showAll.apply(this); };
	this.showTab = function (index) { methods.showTab(index); };
	this.activeTabIndex = function () { return options.activeTabIndex; };

	$.fn.monoTabs = function (opts, method) {
		var defaults = {
			cookie_name: null,
			activeTabIndex: 0
		};
		options = $.extend(defaults, opts);
		return methods.init.apply(this);
		//return this;
	};

})(jQuery);



// cookie thingy
/*!
* jQuery Cookie Plugin
* https://github.com/carhartl/jquery-cookie
*
* Copyright 2011, Klaus Hartl
* Dual licensed under the MIT or GPL Version 2 licenses.
* http://www.opensource.org/licenses/mit-license.php
* http://www.opensource.org/licenses/GPL-2.0
*/
(function ($) {
	$.cookie = function (key, value, options) {

		// key and at least value given, set cookie...
		if (arguments.length > 1 && (!/Object/.test(Object.prototype.toString.call(value)) || value === null || value === undefined)) {
			options = $.extend({}, options);

			if (value === null || value === undefined) {
				options.expires = -1;
			}

			if (typeof options.expires === 'number') {
				var days = options.expires,
                    t = options.expires = new Date();
				t.setDate(t.getDate() + days);
			}

			value = String(value);

			return (document.cookie = [
                encodeURIComponent(key), '=', options.raw ? value : encodeURIComponent(value),
                options.expires ? '; expires=' + options.expires.toUTCString() : '', // use expires attribute, max-age is not supported by IEoptions.path ? '; path=' + options.path : '',
                options.domain ? '; domain=' + options.domain : '',
                options.secure ? '; secure' : ''
                ].join(''));
		}

		// key and possibly options given, get cookie...
		options = value || {};
		var decode = options.raw ?
        function (s) {
        	return s;
        } : decodeURIComponent;

		var pairs = document.cookie.split('; ');
		for (var i = 0, pair; pair = pairs[i] && pairs[i].split('='); i++) {
			if (decode(pair[0]) === key) return decode(pair[1] || ''); // IE saves cookies with empty string as "c; ", e.g. without "=" as opposed to EOMB, thus pair[1] may be undefined
		}
		return null;
	};
})(jQuery);