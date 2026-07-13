import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { CardToDo } from "../../components/card-to-do/card-to-do";
import { CommonModule } from '@angular/common';
import { CardsService } from '../../service/API/cards/cards.service';
import { Header } from '../../components/header/header';
import { AddChangCard, Card_Action } from "../../module/add-chang-card/add-chang-card";
import { FormsModule } from "@angular/forms";
import { HistoryService } from '../../service/history/history.service';

@Component({
  selector: 'app-home',
  imports: [CardToDo, Header, CommonModule, AddChangCard, FormsModule],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home implements OnInit {
  Rest = inject(CardsService);
  
  size: number = 20;
  from: number = 0;
  category: string = '';

  history: string[] = [];

  
  userName: string = 'Max';
  showAddCard: boolean = false;
  cards: card[] = [];
  today: Date = new Date();

  Add = Card_Action.Add;


  constructor(private historyService: HistoryService) {}

  ngOnInit(): void {
    this.getCards();
    this.from += 20;

    this.history = this.historyService.GetSearchHistory();
  }


  openAddCardaPanel(): void {
    this.showAddCard = true;
  }

  closeCard(): void {
    this.showAddCard = false;
  }

  addCard(card: card) {
    this.cards.unshift(card);
    this.showAddCard = false;
  }


  getCards() {
    this.Rest.GetCards(this.size, this.from, this.category).subscribe({
      next: (response) => {
        this.cards = [...this.cards, ...response.cards];
      },
      error: (err) => console.error(err)
    });
  }

  Delete(card: card) {
    this.cards = this.cards.filter(c => c.id !== card.id);
    this.showAddCard = false;
  }

  search(input: HTMLInputElement) {
    input.blur();
    this.cards = [];
    this.from = 0;
    this.Rest.GetCards(this.size, this.from, input.value.toLowerCase()).subscribe({
      next: (response) => {
        this.cards = [...response.cards];

        this.historyService.SaveSearchHistory(input.value.toLowerCase());

        this.history = this.historyService.GetSearchHistory();
      },
      error: (err) => console.error(err)
    });
  }
}
