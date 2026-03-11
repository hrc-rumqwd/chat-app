function onClose(e) {
    const modal = $(e).parent('div.modal');
    modal.toggleClass('is-hidden');
}