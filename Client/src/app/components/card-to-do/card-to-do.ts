import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AddChangCard, Card_Action } from "../../module/add-chang-card/add-chang-card";
import { Router } from '@angular/router';

@Component({
  selector: 'app-card-to-do',
  imports: [AddChangCard],
  templateUrl: './card-to-do.html',
  styleUrl: './card-to-do.scss',
})
export class CardToDo {
  @Input() card!: card;
  @Output() delete = new EventEmitter<card>();

  cardColor: string = '';
  showAddCard: boolean = false;
  Change = Card_Action.Change;

  constructor(private router: Router) { }

  openCard() {
    this.router.navigate([`c/${this.card.id}`])
  }

  editCard() {
    this.showAddCard = true;
  }

  closeCard(): void {
    this.showAddCard = false;
  }

  Updated(card: card) {
    this.card = card;
    this.showAddCard = false;
  }

  Delete(card: card) {
    this.delete.emit(card);
    this.showAddCard = false;
  }
}
