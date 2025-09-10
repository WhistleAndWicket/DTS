import { CommonModule } from '@angular/common';
import {
  AfterViewInit,
  Component,
  Input,
  OnInit,
  ViewChild,
} from '@angular/core';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { Router } from '@angular/router';

import { Observable } from 'rxjs';

import { TaskItemResponseDto } from '../../services/dts-api';
import { TaskItemStateService } from '../../services/task-item-state.service';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [
    CommonModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatTableModule,
  ],
  templateUrl: './task-list.component.html',
  styleUrl: './task-list.component.scss',
})
export class TaskListComponent implements AfterViewInit, OnInit {
  /** Paginator reference for the table. */
  @ViewChild(MatPaginator) paginator = {} as MatPaginator;
  /** Observable of TaskItems to display in the list. */
  @Input() taskItems$!: Observable<TaskItemResponseDto[]>;

  /** Data source for the Material table. */
  dataSource = new MatTableDataSource<TaskItemResponseDto>([]);
  /** Columns to display in the table. */
  displayedColumns: string[] = [
    'id',
    'title',
    'description',
    'status',
    'dueDate',
  ];
  /** Observable indicating if data is currently being loaded. */
  isLoading$!: Observable<boolean>;
  /** Page size options for the paginator, dynamically set based on data. */
  pageSizeOptions: number[] = [];

  constructor(
    private router: Router,
    private taskItemStateService: TaskItemStateService
  ) {}

  ngAfterViewInit() {
    // Attach paginator after setting data.
    if (this.paginator) {
      this.dataSource.paginator = this.paginator;
    }
  }

  ngOnInit() {
    this.isLoading$ = this.taskItemStateService.isLoading$;

    // Subscribe to the taskItems$ observable to update the table data.
    this.taskItems$?.subscribe((items) => {
      this.dataSource.data = items || [];

      // Dynamically set pageSizeOptions.
      const maxPageSize = Math.ceil(items.length / 2);
      this.pageSizeOptions = [5, 10, maxPageSize].filter(
        (v) => v <= items.length
      );
    });
  }

  /**
   * Handles row click to navigate to task detail.
   * @param task The task item that was clicked.
   */
  onRowClick(task: TaskItemResponseDto) {
    this.taskItemStateService.selectTaskById(task.id ?? 0);
    this.router.navigate(['/task-item', task.id]);
  }
}
