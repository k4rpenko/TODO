import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class HistoryService {

  SaveSearchHistory(value: string): string[] {
    value = value.trim().toLowerCase();

    if (!value) {
      return this.GetSearchHistory();
    }

    let history: string[] = JSON.parse(
      localStorage.getItem('searchHistory') || '[]'
    );

    history = history.filter(x => x !== value);

    history.unshift(value);

    history = history.slice(0, 3);

    localStorage.setItem('searchHistory', JSON.stringify(history));

    return history;
  }

  GetSearchHistory(): string[] {
    return JSON.parse(localStorage.getItem('searchHistory') || '[]');
  }

  ClearSearchHistory(): void {
    localStorage.removeItem('searchHistory');
  }
}