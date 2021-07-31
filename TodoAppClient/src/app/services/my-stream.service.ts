import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { Todo } from '../models/todo';

@Injectable({
  providedIn: 'root'
})
export class MyStreamService {
  private todoSource = new ReplaySubject<Todo>(1);
  todo$ = this.todoSource.asObservable();
  
  constructor() { }

  set Todo(value: Todo) {
    this.todoSource.next(value);
  }
}
