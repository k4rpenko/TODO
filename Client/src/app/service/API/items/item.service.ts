import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ItemService {
    http = inject(HttpClient)
    constructor() { }

    GetItems(size: number, from: number, cardId: string){
        return this.http.get<{items: item[]}>(`/api/CardItem?size=${size}&from=${from}&cardID=${cardId}`,
            {
                withCredentials: true
            }
        );
    }

    PostCreatItem(title: string, cardId: string) {
        const json = {
            "title": title,
            "cardId": cardId
        };

        return this.http.post<{item: item}>(`api/CardItem/Create`, json, {
            headers: { 'Content-Type': 'application/json' },
            withCredentials: true
        });
    }

    ChangeIsCompleted(id: string, cardId: string) {
        const json = {
            "id": id,
            "cardId": cardId,
        };

        return this.http.patch<{item: item}>(`api/CardItem/IsCompleted`, json, {
            headers: { 'Content-Type': 'application/json' },
            withCredentials: true
        });
    }

    ChangeItem(id: string, title:string, cardId: string) {
        const json = {
            "id": id,
            "title": title,
            "cardId": cardId,
        };

        return this.http.patch<{item: item}>(`api/CardItem/Change`, json, {
            headers: { 'Content-Type': 'application/json' },
            withCredentials: true
        });
    }


    DeleteItem(ItemId: string, cardId: string) {
        const json = {
            "id": ItemId,
            "cardId": cardId
        };

        return this.http.delete(`api/CardItem/Delete`, {
            body: json,
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true
        });
    }
}