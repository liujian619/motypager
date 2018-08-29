;(function($, window, document, undefined) {
	"use strict";
	var MotyPager = function(element, options) {
		this.$element = $(element),
		this.defaults = {
	        color: "#787d82",
	        disabled_color: "#c8cdd2",
	        hover_color: "#ec1500",
	        cur_bgcolor: "#ec1500",
	        cur_color: "#fff",

	        first: true,
	        last: true,
	        prev: true,
	        next: true,
	        hide_when_only_one: true,
	        pc_style: true,
	        norefresh: false, //是否是无刷新分页，开启则url、cur、total参数设置无效

	        first_text: "首页",
	        last_text: "尾页",
	        prev_text: "上一页",
	        next_text: "下一页",

	        pages_shown: 7,

	        //norefresh为false有效
	        url: function(index) {
	            return "index_" + index;
	        },
	        //norefresh为true有效
	        clicked: function(index, update) {
	            update(1);
	        }
	    },
	    this.opts = $.extend({}, this.defaults, options)
	}

	MotyPager.VERSION = '2.0.0';
	
	/**
	 * 转到指定的页码
	 * @param  {int} cur   [当前页码]
	 * @param  {int} total [总页码数]
	 */
	MotyPager.prototype.pageTo = function(cur, total) {
		var opts = this.opts, root = this.$element;
		if (opts.norefresh) {
            opts.clicked(cur, function(totalPages) {
                build(root, opts, cur, totalPages);                     
        	});
        } else {
            build(root, opts, cur, total);
		}
	}
	
	
	/**
     * 获取待显示的页码。
     * @param cur  当前处于第几页
     * @param total  总页面数
     * @param pages_shown  将要显示的最多页码数
     * @return 页码数组
     * eg: getPagesToShow(5, 10, 5) 则返回 [3,4,5,6,7]
     */
    function getPagesToShow(cur, total, pages_shown) {
        var ret = [];
        var half = Math.floor((pages_shown - 1) / 2);
        var pad = cur + half > total ? (cur + half - total) : 0;
        var min = Math.max(1, cur - half - pad);
        for (var i = min; i <= cur; i++) {
            ret.push(i);
        }
        var max = Math.min(total, cur + pages_shown - ret.length);
        for (var i = cur + 1; i <= max; i++) {
            ret.push(i);
        }
        return ret;
    }

    /**
     * 创建分页部件的核心。
     * @param $root  分页部件的根节点，使用jQuery包裹了一下的对象
     * @param cur  当前页码
     * @param total  总页码数
     * @return jQuery包裹的生成的核心HTML对象
     */
    function createCore($root, opts, cur, total) {
    	var html = "";
    	if (opts.first) {
            if (cur === 1) {
                html += "<span class='disabled_page'>{0}</span>".replace("{0}", opts.first_text);
            } else {
                html += "<a href='#' data-index='{0}'>{1}</a>".replace("{0}", 1).replace("{1}", opts.first_text);
            }
        }
        if (opts.prev) {
            if (cur === 1) {
                html += "<span class='disabled_page'>{0}</span>".replace("{0}", opts.prev_text);
            } else {
                html += "<a href='#' data-index='{0}'>{1}</a>".replace("{0}", cur - 1).replace("{1}", opts.prev_text);
            }
        }

        if (opts.pc_style) {
            var pages = getPagesToShow(cur, total, opts.pages_shown)
            for (var i = 0; i < pages.length; i++) {
                if (cur === pages[i]) {
                    html += "<a href='javascript:void(0)' class='active_1604261319'>{0}</a>".replace("{0}", pages[i]);
                } else {
                    html += "<a href='#' data-index='{0}'>{1}</a>".replace("{0}", pages[i]).replace("{1}", pages[i]);
                }
            }
        } else {
            html += "<select>";
            for (var i = 0; i < total; i++) {
                if (cur === i + 1) {
                    html += "<option selected='selected' value='{0}'>{1}</option>".replace("{0}", i + 1).replace("{1}", i + 1);
                } else {
                    html += "<option value='{0}'>{1}</option>".replace("{0}", i + 1).replace("{1}", i + 1);
                }
            }
            html += "</select>";
        }

        if (opts.next) {
            if (cur === total) {
                html += "<span class='disabled_page'>{0}</span>".replace("{0}", opts.next_text);
            } else {
                html += "<a href='#' data-index='{0}'>{1}</a>".replace("{0}", cur + 1).replace("{1}", opts.next_text);
            }
        }
        if (opts.last) {
            if (cur === total) {
                html += "<span class='disabled_page'>{0}</span>".replace("{0}", opts.last_text);
            } else {
                html += "<a href='#' data-index='{0}'>{1}</a>".replace("{0}", total).replace("{1}", opts.last_text);
            }
        }

        var $obj = $("<div class='motypc_1604261319'>" + html + "</div>");
        if (!opts.pc_style) {
            var selects = $obj.find("select");
            $(selects).on("change", function() {
                var index = $(this).val();
                if (!opts.norefresh) {
                    window.location = opts.url(index);
                } else {
                    opts.clicked(index, function(totalPages) {
                        build($root, opts, index, Math.max(totalPages, 1));                       
                    });
                }
            });
        }
        var aas = $obj.find("a[href='#']");
        for (var i = 0; i < aas.length; i++) {
            var index = $(aas[i]).attr("data-index");
            $(aas[i]).attr("href", opts.norefresh ? "javascript:" : opts.url(index));
        }
        $obj.find(".disabled_page").css("color", opts.disabled_color);
        $obj.find("a").css("color", opts.color).mouseover(function() {
            $obj.find(this).not("a.active_1604261319").css("color", opts.hover_color).css("border-color", opts.hover_color);
        }).mouseout(function() {
            $obj.find(this).not("a.active_1604261319").css("color", opts.color).css("border-color", "transparent");
        });
        $obj.find("a.active_1604261319").css("background-color", opts.cur_bgcolor).css("color", opts.cur_color);

        $obj.find("a:not('.active_1604261319')").click(function() {
            if (opts.norefresh) {
                var index = $(this).attr("data-index");
                opts.clicked(index, function(totalPages) {
                    build($root, opts, index, Math.max(totalPages, 1));                       
                });
            }
        });
        return $obj;
    }

	/**
     * 创建分页部件。
     * @param $root  分页部件的根节点，使用jQuery包裹了一下的对象
     * @param cur  当前页码
     * @param total  总页码数
     */
    function build($root, opts, cur, total) {
    	cur = parseInt(cur);
        total = parseInt(total);
        if (typeof cur !== "number" || !/^[1-9]\d*$/.test(cur)) {
            throw new Error("The second parameter must be a positive integer.");
        }
        if (typeof total !== "number" || !/^[1-9]\d*$/.test(total)) {
            throw new Error("The third parameter must be a positive integer.");
        }
        if (cur > total) {
            throw new Error("The second parameter must be less than or equal to the third one.");
        }
        $root.html("");
        if (total <= 1 && this.opts.hide_when_only_one) {
            $root.append("");
        } else {
            $root.append(createCore($root, opts, cur, total));
        }
    }

    function Plugin(option, cur, total){
    	return this.each(function () {
	        var $this   = $(this);
	        var data    = $this.data('moty.motypager');
	        var options = typeof option === 'object' && option;

	        if (!data) {
	        	$this.data('moty.motypager', (data = new MotyPager(this, options)));
			}

			if (typeof option === 'string') {
	        	if (option === 'pageTo') data.pageTo(cur, total);
	        	else throw new Error("The command('" + option + "') is invalid.");
	    	}
	    })
    }

    var old = $.fn.motypager;
    $.fn.motypager = Plugin;

	$.fn.motypager.noConflict = function () {
	    $.fn.motypager = old;
	    return this;
	}
})(jQuery, window, document);
