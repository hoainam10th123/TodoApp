import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import {CdkDragDrop, moveItemInArray, transferArrayItem} from '@angular/cdk/drag-drop';
import { Status, Todo } from 'src/app/models/todo';
import { TodoService } from 'src/app/services/todo.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { AddEditModalComponent } from '../modal/add-edit-modal/add-edit-modal.component';
import { MyStreamService } from 'src/app/services/my-stream.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-todo',
  templateUrl: './todo.component.html',
  styleUrls: ['./todo.component.css']
})
export class TodoComponent implements OnInit, OnDestroy {
  modalRef: BsModalRef;
  
  constructor(private toastr: ToastrService, private myStream: MyStreamService, private todoService: TodoService, private modalService: BsModalService) { }

  ngOnInit(): void {
    this.loadTodos();
    this.myStream.todo$.subscribe(todo=>{
      switch (todo.status) {
        case Status.Cancel: {
          if(this.todoCancel){
            this.todoCancel.push(todo);
          }
          break;
        }
        case Status.Doing: {
          if(this.todoDoing){
            this.todoDoing.push(todo);
          }
          break;
        }
        case Status.Done: {
          if(this.todoDone){
            this.todoDone.push(todo);
          }
          break;
        }
      }      
    })
  }

  loadTodos(){
    this.todoService.getCancelTodos().subscribe(todos=>{
      this.todoCancel = todos;
    });
    this.todoService.getDoingTodos().subscribe(todos=>{
      this.todoDoing = todos;
    });
    this.todoService.getDoneTodos().subscribe(todos=>{
      this.todoDone = todos;
    });
  }

  todoCancel: Todo[] = [];

  todoDone: Todo[] = [];

  todoDoing: Todo[] = [];

  drop(event: CdkDragDrop<Todo[]>) {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(event.previousContainer.data,
                        event.container.data,
                        event.previousIndex,
                        event.currentIndex);

      let splitId = event.container.id.split('cdk-drop-list-');
      let lengthId = parseInt(splitId[1]) + 1;
      let model = event.container.data[event.currentIndex];
      this.chosenTodoContainer(lengthId, model);
      //console.log(event.container.data[event.currentIndex]);//current data picked up
    }
  }

  runQuartz(){
    this.todoService.runQuartz().subscribe(()=>{
      this.toastr.success('Runing Quartz');
    })
  }

  chosenTodoContainer(length: number, todo: Todo){
    switch (length % 3) {// 3 Todo container
      case 1: {//first todo: todoCancel
        this.todoService.updateStatusTodo(todo.id, Status.Cancel).subscribe(()=>{
          console.log("OK");
        });
        break;
      }
      case 2: {//second todo: todoDoing
        this.todoService.updateStatusTodo(todo.id, Status.Doing).subscribe(()=>{
          console.log("OK");
        });
        break;
      }
      case 0: {//third todo: todoDone
        this.todoService.updateStatusTodo(todo.id, Status.Done).subscribe(()=>{
          console.log("OK");
        });
        break;
      }
      default: {
        console.log('default');        
        break;
      }
    }
  }

  openModal(){
    // const initialState = {  
    //   postId: id,
    //   isPost: isPost
    // };
    this.modalRef = this.modalService.show(AddEditModalComponent);
  }

  @HostListener('unloaded')
  ngOnDestroy(): void {
    console.log('TodoComponent destroyed');
    this.todoCancel = [];
    this.todoDoing = [];
    this.todoDone = [];   
  }
}
