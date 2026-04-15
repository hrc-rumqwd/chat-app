import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CustomModal } from './custom-modal/custom-modal';

@NgModule({
  declarations: [
    CustomModal // Declare it here so the module knows it exists
  ],
  imports: [
    CommonModule
  ],
  exports: [
    CustomModal // EXPORT it so other modules can use it
  ]
})
export class SharedModule { }