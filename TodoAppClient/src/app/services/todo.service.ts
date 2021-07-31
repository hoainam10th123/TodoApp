import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Status, Todo } from '../models/todo';

@Injectable({
  providedIn: 'root'
})
export class TodoService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getCancelTodos(){
    return this.http.get<Todo[]>(this.baseUrl+'Todo/get-cancel-todos');
  }

  getDoneTodos(){
    return this.http.get<Todo[]>(this.baseUrl+'Todo/get-done-todos');
  }

  getDoingTodos(){
    return this.http.get<Todo[]>(this.baseUrl+'Todo/get-doing-todos');
  }

  runQuartz(){
    return this.http.get(this.baseUrl+'Todo/quartz');    
  }

  addTodo(todo: any){
    switch (todo.status) {
      case "0": {
        todo.status = 0;
        break;
      }
      case "1": {
        todo.status = 1;
        break;
      }
      case "2": {
        todo.status = 2;
        break;
      }
      default: {
        console.log('default');        
        break;
      }
    }
    return this.http.post(this.baseUrl+'Todo', todo);
  }

  updateStatusTodo(todoId: number, status: Status){
    return this.http.put(this.baseUrl+'Todo?todoId='+todoId+'&status='+ status, {});
  }
}
