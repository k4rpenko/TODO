import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CardsService {
    http = inject(HttpClient)
    constructor() { }

    PostCreatCard(card: card) {
        return this.http.post<{card: card}>(`api/Card/Create`, card, {
            headers: { 'Content-Type': 'application/json' },
            withCredentials: true
        });
    }

    PostChangeCard(card: card) {
        return this.http.patch<{card: card}>(`api/Card/Change`, card, {
            headers: { 'Content-Type': 'application/json' },
            withCredentials: true
        });
    }

    GetCards(size: number, from: number, category: string){
        return this.http.get<{cards: card[]}>(`/api/Card?size=${size}&from=${from}&category=${category}`,
            {
                withCredentials: true
            }
        );
    }

    GetCardById(cardId: string){
        return this.http.get<{card: card}>(`api/Card/${cardId}`,
            {
                withCredentials: true
            }
        );
    }

    DeleteCard(card: card) {
        return this.http.delete('api/Card/Delete', {
            body: card,
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true
        });
    }
}