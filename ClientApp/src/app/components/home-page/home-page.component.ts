import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

import {
  CreateTaskItemCommandDto,
  TaskItemResponseDto,
} from '../../services/dts-api';
import { CreateTaskDialogComponent } from '../create-task-dialog/create-task-dialog.component';
import { CreateTaskDialogData as CreateTaskDialogResultData } from '../../models/create-task-dialog-data.model';
import { TaskItemStateService } from '../../services/task-item-state.service';
import { TaskListComponent } from '../task-list/task-list.component';

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [CommonModule, TaskListComponent, MatButtonModule],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss',
})
export class HomePageComponent implements OnInit {
  /** Observable indicating if data is currently being loaded. */
  isLoading$!: Observable<boolean>;
  /** Observable of TaskItems to display in the list. */
  taskItems$!: Observable<TaskItemResponseDto[]>;

  constructor(
    private dialog: MatDialog,
    private router: Router,
    private taskItemStateService: TaskItemStateService
  ) {}

  ngOnInit(): void {
    // Load all the task and get the observable.
    this.taskItemStateService.loadAll();

    // Subscribe to the observables from the state service.
    this.taskItems$ = this.taskItemStateService.taskItems$;
    this.isLoading$ = this.taskItemStateService.isLoading$;
  }

  /**
   * Opens the Create Task dialog and handles the result.
   * If a new task is created, navigates to its detail page.
   */
  onCreateTask(): void {
    // Open the CreateTaskDialogComponent as a modal dialog.
    const dialogRef = this.dialog.open(CreateTaskDialogComponent, {
      backdropClass: `dialog-backdrop`,
      width: `480px`,
      disableClose: true,
    });

    dialogRef.afterClosed().subscribe((result: CreateTaskDialogResultData) => {
      if (result) {
        const createCommand: CreateTaskItemCommandDto = {
          title: result.title,
          description: result.description,
          dueDate: result.dueDate,
          status: result.status,
        };

        // Call the service to create the task.
        this.taskItemStateService.createTask(createCommand).subscribe({
          next: (created) => {
            if (created?.id) {
              // Navigate to the newly created task's detail page.
              this.router.navigate(['/task-item', created.id]);

              // Select the newly created task in the state service.
              this.taskItemStateService.selectTaskById(created.id);
            }
          },
          error: () => console.error($localize`Failed to create task`),
        });
      }
    });
  }
}
