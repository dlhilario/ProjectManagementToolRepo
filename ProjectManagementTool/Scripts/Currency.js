// Jquery Dependency
(function (root, factory) {
    if (typeof define === 'function' && define.amd) {
        // AMD. Register as an anonymous module.
        define(['jquery'], factory);
    } else if (typeof exports === 'object') {
        // Node. Does not work with strict CommonJS, but
        // only CommonJS-like environments that support module.exports,
        // like Node.
        module.exports = factory(require('jquery'));
    } else {
        // Browser globals (root is window)
        root.Currency = factory(root.jQuery);
    }
}(this, function ($) {

    function Currency(options) {
        this.init();
        this.options = $.extend({}, this.constructor.defaults);
        this.call(options);
    }

    Currency.defaults = {
        dollarSymbol: "$",
    }

    Currency.prototype.init = function () {
        var self = this;
        // Both enable and build methods require the body tag to be in the DOM.
        $(document).ready(function () {
            self.build($("input[data-type='currency']"));
            self.enable();
        });
    };

    Currency.prototype.enable = function () {
        var self = this;
        $("input[data-type='currency']").on({
            keyup: function () {
                self.formatCurrency($(this));
            },
            blur: function () {
                self.formatCurrency($(this), "blur");
            },
            mousemove: function () {
                self.formatCurrency($(this));
            }
        });
    };

    Currency.prototype.build = function (element) {
        var currectElement = element;

        currectElement.each(function (evt, element) {
            var input_currency = `<div class="input-group mb-3">
            <div class="input-group-prepend">
                <span class="input-group-text">$</span>
            </div>
                ${element.outerHTML}            
            </div>`;
           $(element).replaceWith(input_currency);
        })

    };
    Currency.prototype.call = function (options) {
        $.extend(this.options, options);
    }

    Currency.prototype.formatNumber = function (n) {
        // format number 1000000 to 1,234,567
        return n.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",")
    }

    Currency.prototype.formatCurrency = function (input, blur) {
        var self = this;
        // appends $ to value, validates decimal side
        // and puts cursor back in right position.

        // get input value
        var input_val = input.val();

        // don't validate empty input
        if (input_val === "") { return; }

        // original length
        var original_len = input_val.length;

        // initial caret position 
        var caret_pos = input.prop("selectionStart");

        // check for decimal
        if (input_val.indexOf(".") >= 0) {

            // get position of first decimal
            // this prevents multiple decimals from
            // being entered
            var decimal_pos = input_val.indexOf(".");

            // split number by decimal point
            var left_side = input_val.substring(0, decimal_pos);
            var right_side = input_val.substring(decimal_pos);

            // add commas to left side of number
            left_side = self.formatNumber(left_side);

            // validate right side
            right_side = self.formatNumber(right_side);

            // On blur make sure 2 numbers after decimal
            if (blur === "blur") {
                right_side += "00";
            }

            // Limit decimal to only 2 digits
            right_side = right_side.substring(0, 2);

            // join number by .
            input_val = self.options.dollarSymbol + left_side + "." + right_side;

        } else {
            // no decimal entered
            // add commas to number
            // remove all non-digits
            input_val = self.formatNumber(input_val);
            input_val = self.options.dollarSymbol + input_val;

            // final formatting
            if (blur === "blur") {
                input_val += ".00";
            }
        }

        // send updated string to input
        input.val(input_val);

        // put caret back in the right position
        var updated_len = input_val.length;
        caret_pos = updated_len - original_len + caret_pos;
        input[0].setSelectionRange(caret_pos, caret_pos);
    }

    return new Currency();

}));



