import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Todo } from 'src/app/models/todo';
import { MyStreamService } from 'src/app/services/my-stream.service';
import { TodoService } from 'src/app/services/todo.service';

@Component({
  selector: 'app-add-edit-modal',
  templateUrl: './add-edit-modal.component.html',
  styleUrls: ['./add-edit-modal.component.css']
})
export class AddEditModalComponent implements OnInit {
  todoForm: FormGroup;
  maxDate: Date;

  list = [
    {key: 0, value: 'Done'},
    {key: 1, value: 'Cancel'},
    {key: 2, value: 'Doing'}
  ];
  
  constructor(private myStream: MyStreamService, public bsModalRef: BsModalRef, private fb: FormBuilder, private todoService: TodoService) { }

  ngOnInit(): void {    
    this.khoiTaoForm();
    this.maxDate = new Date();    
  }

  khoiTaoForm(){
    this.todoForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(30)]],
      description: [''],
      startDate: ['', Validators.required],
      endDate: ['', [Validators.required, this.matchValues('startDate')]],
      status: [2]
    })
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value >= control?.parent?.controls[matchTo].value 
        ? null : {isMatching: true}
    }
  }

  save(){
    this.todoService.addTodo(this.todoForm.value).subscribe((todo : Todo)=>{
      this.myStream.Todo = todo;
      this.bsModalRef.hide();
    })
  }

}
