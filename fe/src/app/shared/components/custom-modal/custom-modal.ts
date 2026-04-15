import { Component, EventEmitter, input, Input, Output } from '@angular/core';

@Component({
  selector: 'app-custom-modal',
  standalone: false,
  templateUrl: './custom-modal.html',
  styleUrl: './custom-modal.css',
})
export class CustomModal {
  @Input()
  title = 'Custom Modal Title';

  @Output() onSuccessTrigger = new EventEmitter<void>();
  onOkClicked() {
    this.onSuccessTrigger.emit();
  }

  @Output() onCloseTrigger = new EventEmitter<void>();
  onCloseClicked() {
    // Close the modal

    // Trigger event if not null
    if(this.onCloseTrigger)
      this.onCloseTrigger.emit();
  }
}
