import { ChangeDetectorRef, Component, HostListener, inject, Input, OnInit } from '@angular/core';
import { Header } from "../../components/header/header";
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ItemService } from '../../service/API/items/item.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { CardsService } from '../../service/API/cards/cards.service';

@Component({
  selector: 'app-card-item',
  imports: [Header, FormsModule, CommonModule],
  templateUrl: './card-item.html',
  styleUrl: './card-item.scss',
})
export class CardItem implements OnInit{
  Rest = inject(ItemService);
  Rest_Card = inject(CardsService);

  card!: card;
  isLoading = true;

    
  size: number = 20;
  from: number = 0;
  cardId: string = '';

  constructor(private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.cardId = this.route.snapshot.paramMap.get('CardItemId') ?? '';

    this.GetCardById();
    this.GetItems();
  }

  
  // @HostListener('document:click', ['$event'])
  // onDocumentClick(event: MouseEvent) {
  //   const target = event.target as HTMLElement;

  //   if (target.tagName !== 'INPUT') {
  //     (document.activeElement as HTMLElement)?.blur();
  //   }
  // }

  newTaskText: string = '';



  addNewTask() {
    if (this.newTaskText.trim()) {
      this.Rest.PostCreatItem(this.newTaskText, this.card.id!).subscribe({
        next: (value) => {
          let item = value.item

          this.card.items!.push(item);
          this.newTaskText = '';
        },
        error: (err) => {
          //this.router.navigate(['home']);
        }
      });
    }
  }

  back() {
    window.history.back();
  }

  GetCardById() {
    this.Rest_Card.GetCardById(this.cardId).subscribe({
      next: (value) => {
        this.card = value.card
      },
      error: (err) => {
        this.router.navigate(['home']);
      }
    });
  }

  GetItems() {
    this.Rest.GetItems(this.size, this.from, this.cardId).subscribe({
      next: (value) => {
        this.card.items = value.items
        this.isLoading = false
      },
      error: (err) => {
        //this.router.navigate(['home']);
      }
    });
  }

  deleteItem(itemId: string){
    this.Rest.DeleteItem(itemId, this.cardId).subscribe({
      next: (value) => {
        this.card.items = this.card.items?.filter(x => x.id != itemId);
      },
      error: (err) => {
        //this.router.navigate(['home']);
      }
    });
  }

  IsCompleted(itemId: string){
    this.Rest.ChangeIsCompleted(itemId, this.cardId).subscribe({
      next: (value) => {
        let item = this.card.items?.find(x => x.id === itemId);
        item!.IsCompleted = !item!.IsCompleted;
      },
      error: (err) => {
        //this.router.navigate(['home']);
      }
    });
  }

  ChengItem(item: item){
    this.Rest.ChangeItem(item.id!, item.title, this.cardId).subscribe({
      next: (value) => {
        let res = this.card.items!.find(x => x.id === item.id);
        res = value.item;
      },
      error: (err) => {
        //this.router.navigate(['home']);
      }
    });
  }
}
