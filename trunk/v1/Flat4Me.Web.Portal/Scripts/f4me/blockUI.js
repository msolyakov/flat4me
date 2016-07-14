/**
* Block UI object functionality
*/
var BlockUI = {
    blockMessage: '<i class="fa fa-refresh fa-4x fa-spin" style="color: #FFF"></i>',
    /**
	* Block UI element or page
	*
	* @param element to block, if element == null then block page
	*/
    block: function (element) {
        $.blockUI.defaults.css.border = '';
        $.blockUI.defaults.css.backgroundColor = '';
        $.blockUI.defaults.message = this.blockMessage;

        if (element != null) {
            element.block();
        } else {
            $.blockUI();
        }
    },

    /**
	* Unblock UI element or page
	*
	* @param element to unblock, if element == null then unblock page
	*/
    unblock: function (element) {
        if (element != null) {
            element.unblock();
        } else {
            $.unblockUI();
        }
    }
}