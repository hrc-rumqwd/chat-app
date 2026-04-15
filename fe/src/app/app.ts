import { Component, signal } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.css'
})
export class App {
  
  isModalVisible = false;

  handleCreateNewConversation() {
    this.isModalVisible = true;
  }

  onCloseModal() {
    this.isModalVisible = false;
  }

  onCreateNewConversation() {
    this.isModalVisible = false;
  }
}
