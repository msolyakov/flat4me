var ModalForm = {
    handleSubmit: function (actionOnUpdate) {
        $.validator.unobtrusive.parse("form");

        $("#modal form").submit(function (event) {
            event.preventDefault();

            var form = $("#modal form");
            var content = $("#modal .modal-content");

            form.validate();

            if (form.valid()) {
                BlockUI.block(content);

                $.ajax({
                    url: form.prop("action"),
                    data: form.serialize(),
                    type: "POST",
                    success: function (data) {
                        BlockUI.unblock(content);

                        if (data == true) {
                            location.reload();
                        }
                        else {
                            content.html(data);

                            ModalForm.handleSubmit(actionOnUpdate);

                            $.validator.unobtrusive.parse(content);

                            if (actionOnUpdate != undefined) {
                                actionOnUpdate();
                            }
                        }
                    },
                    error: function (data) {
                        BlockUI.unblock(content);
                    }
                });
            }

            return false;
        });
    },

    // Make check boxes in modal form to cool bootstrap switch
    makeSwitch: function () {
        $("#modal input[type=checkbox]").bootstrapSwitch("destroy");
        $("#modal input[type=checkbox]").bootstrapSwitch({
            onColor: "success",
            onText: "Да",
            offText: "Нет"
        });
    }
}