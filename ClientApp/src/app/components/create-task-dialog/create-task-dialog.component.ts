import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { CreateTaskDialogData } from '../../models/create-task-dialog-data.model';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { TaskItemStatus } from '../../services/dts-api';

@Component({
  selector: 'app-create-task-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatDatepickerModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    ReactiveFormsModule,
  ],
  templateUrl: './create-task-dialog.component.html',
  styleUrl: './create-task-dialog.component.scss',
})
export class CreateTaskDialogComponent implements OnInit {
  /** The possible statuses for a TaskItem. */
  statuses = Object.values(TaskItemStatus).filter((v) => typeof v === 'string');
  /** The form group for the TaskItem fields. */
  taskForm!: FormGroup;

  constructor(
    private dialogRef: MatDialogRef<CreateTaskDialogComponent>,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    // Initialize the form with validators.
    this.taskForm = this.fb.group({
      title: ['', Validators.required],
      description: [''],
      dueDate: [null, Validators.required],
      status: ['Created', Validators.required],
    });
  }

  /**
   * Saves the form data and closes the dialog, returning the entered data.
   * Only called if the form is valid.
   */
  save() {
    if (this.taskForm.valid) {
      const dialogData: CreateTaskDialogData = {
        title: this.taskForm.value.title,
        description: this.taskForm.value.description,
        dueDate: this.taskForm.value.dueDate,
        status: this.taskForm.value.status,
      };

      this.dialogRef.close(dialogData); // send data back
    }
  }

  /** Closes the dialog without saving. */
  cancel() {
    this.dialogRef.close();
  }
}
