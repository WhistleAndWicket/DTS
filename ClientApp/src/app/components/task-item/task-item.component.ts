import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialog } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { ReactiveFormsModule } from '@angular/forms';

import { Observable, tap } from 'rxjs';

import { ConfirmationDialogComponent } from '../confirmation-dialog/confirmation-dialog.component';
import { ConfirmationDialogData } from '../../models/confirmation-dialog-data.model';
import { TaskItemResponseDto, TaskItemStatus } from '../../services/dts-api';
import { TaskItemStateService } from '../../services/task-item-state.service';

@Component({
  selector: 'app-task-item',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatProgressSpinnerModule,
    MatSelectModule,
    ReactiveFormsModule,
  ],
  templateUrl: './task-item.component.html',
  styleUrl: './task-item.component.scss',
})
export class TaskItemComponent implements OnInit, OnDestroy {
  /** The possible statuses for a TaskItem. */
  statuses = Object.values(TaskItemStatus).filter((v) => typeof v === 'string');
  /** The observable for the current TaskItem. */
  task$: Observable<TaskItemResponseDto | null>;
  /** Observable indicating if data is currently being loaded. */
  isLoading$!: Observable<boolean>;
  /** Observable indicating if the status update is in progress. */
  isUpdatingStatus$: Observable<boolean>;
  /** The ID of the current TaskItem. */
  taskId: number = 0;
  /** Flag indicating if the task was not found. */
  taskNotFound: boolean = false;

  constructor(
    private dialog: MatDialog,
    private route: ActivatedRoute,
    private router: Router,
    private taskItemStateService: TaskItemStateService
  ) {
    this.task$ = this.taskItemStateService.selectedTaskItem$;
    this.isUpdatingStatus$ = this.taskItemStateService.isUpdatingStatus$;
  }

  ngOnDestroy(): void {
    this.taskItemStateService.clearSelectedTask();
  }

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.taskId = id;

      // Load and select the task by ID.
      this.taskItemStateService.selectTaskById(id);
      this.isLoading$ = this.taskItemStateService.isLoading$;

      // Subscribe to the selectedTaskItem$ to set taskNotFound flag if needed.
      this.task$ = this.taskItemStateService.selectedTaskItem$.pipe(
        tap((task) => {
          this.taskNotFound = !task; // set flag if task is null
        })
      );
    }
  }

  /** Navigates back to the home page. */
  onBack() {
    this.router.navigate(['/']);
  }

  /**
   * Handles the deletion of a task item after user confirmation.
   * @param taskItem The task item to be deleted.
   */
  onDelete(taskItem: TaskItemResponseDto) {
    // Prepare confirmation dialog data.
    const dialogData: ConfirmationDialogData = {
      confirmationButtonText: $localize`Delete`,
      message: $localize`Are you sure you want to delete the task "${taskItem.title}"? This action cannot be undone.`,
      title: $localize`Delete Task`,
    };

    // Open confirmation dialog.
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      backdropClass: `dialog-backdrop`,
      width: `480px`,
      disableClose: true,
      data: dialogData,
    });

    dialogRef.afterClosed().subscribe((confirmed: boolean) => {
      if (confirmed) {
        // User confirmed deletion, proceed to delete the task.
        this.taskItemStateService.deleteTask(taskItem.id ?? 0);
      }
    });
  }

  /**
   * Handles status change for the task item.
   * @param newStatus The new status selected by the user.
   */
  onStatusChange(newStatus: string) {
    if (this.taskId && newStatus) {
      const updatedStatus =
        TaskItemStatus[newStatus as keyof typeof TaskItemStatus];

      // Call the state service to update the task status.
      this.taskItemStateService.updateTaskStatus(this.taskId, updatedStatus);
    }
  }
}
