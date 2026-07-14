import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { CardsService } from '../../service/API/cards/cards.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

export enum Card_Action{
  Change,
  Add
}

@Component({
  selector: 'app-add-chang-card',
  imports: [CommonModule, FormsModule],
  templateUrl: './add-chang-card.html',
  styleUrl: './add-chang-card.scss',
})
export class AddChangCard implements OnInit{
  Rest = inject(CardsService);
  
  @Input() type!: Card_Action
  @Input() card: card | undefined;
  @Output() closed = new EventEmitter<void>();
  @Output() create = new EventEmitter<card>();
  @Output() updated = new EventEmitter<card>();
  @Output() delete = new EventEmitter<card>();

  Add = Card_Action.Add;
  Change = Card_Action.Change;

  page: number = 0;
  pageSize: number = 4;

  designs = [1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20];
  

  NewCard: card = {
    title: '',
    description: '',
    collor: '#0DA4D3',
    design: 1,
    hashtag: []
  };

  ngOnInit(): void {
    if (this.type === Card_Action.Change && this.card) {
      this.NewCard = structuredClone(this.card);
      this.hashtags = [...this.NewCard.hashtag];
      this.design = this.NewCard.design;
    }
  }

  tagInput = '';
  hashtags: string[] = [];
  design = 1;

  get visibleDesign(){
    const start = this.page * this.pageSize;
    return this.designs.slice(start, start + this.pageSize);
  }

  nextPage(){
    if((this.page + 1) * this.pageSize < this.designs.length){
      this.page++;
    }
  }

  previousPage() {
    if (this.page > 0) {
      this.page--;
    }
  }

  addTag() {
      const tag = this.tagInput.trim().replace('#', '');

      if (!tag) return;

      if (tag.length > 10) {
          alert('Hashtag cannot exceed 10 characters.');
          return;
      }

      if (this.hashtags.includes(tag)) {
          this.tagInput = '';
          return;
      }

      this.hashtags.push(tag);
      this.tagInput = '';
  }

  removeTag(tag: string) {
      this.hashtags = this.hashtags.filter(t => t !== tag);
  }

  close() {
    this.closed.emit();
  }

  AddCardService(){
    this.NewCard.design = this.design;
    this.NewCard.hashtag?.push(...this.hashtags);

    this.Rest.PostCreatCard(this.NewCard).subscribe({
      next: (response) => {
        const card = response.card;
        this.NewCard = card;
        this.create.emit(this.NewCard);
        this.closed.emit();
      }
      // error: (error) => {
        
      // }
    });
  }

  editCard(){
    this.NewCard.design = this.design;
    this.NewCard.hashtag = this.hashtags;

    this.Rest.PostChangeCard(this.NewCard).subscribe({
      next: (response) => {
        this.updated.emit(this.NewCard);
        this.closed.emit();
      }
      // error: (error) => {
        
      // }
    });
  }

  deleteCard() {
    this.Rest.DeleteCard(this.NewCard).subscribe({
      next: (response) => {
        this.delete.emit(this.NewCard);
        this.closed.emit();
      }
      // error: (error) => {
        
      // }
    });
  }
}
