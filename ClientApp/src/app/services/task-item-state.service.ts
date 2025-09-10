import { Injectable } from '@angular/core';
import {
  CreateTaskItemCommandDto,
  TaskItemResponseDto,
  TaskItemService,
  TaskItemStatus,
  UpdateTaskItemStatusCommandDto,
} from './dts-api';
import { BehaviorSubject, finalize, Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class TaskItemStateService {
  //#region Private state management subjects.

  /** Subject indicating if data is being loaded. */
  private _isLoading$ = new BehaviorSubject<boolean>(false);
  /** Subject indicating if a status update is in progress. */
  private _isUpdatingStatus$ = new BehaviorSubject<boolean>(false);
  /** Subject for the currently selected task item. */
  private _selectedTaskItem$ = new BehaviorSubject<TaskItemResponseDto | null>(
    null
  );
  /** Subject for the list of task items. */
  private _taskItems$ = new BehaviorSubject<TaskItemResponseDto[]>([]);

  //#endregion Private state management subjects.

  constructor(private taskItemService: TaskItemService) {}

  /** Observable indicating if data is being loaded. */
  get isLoading$(): Observable<boolean> {
    return this._isLoading$.asObservable();
  }

  /** Observable indicating if a status update is in progress. */
  get isUpdatingStatus$(): Observable<boolean> {
    return this._isUpdatingStatus$.asObservable();
  }

  /** Observable of all task items. */
  get taskItems$(): Observable<TaskItemResponseDto[]> {
    return this._taskItems$.asObservable();
  }

  /** Observable of the selected task item. */
  get selectedTaskItem$(): Observable<TaskItemResponseDto | null> {
    return this._selectedTaskItem$.asObservable();
  }

  /** Array of all task titles, used for validation. */
  get taskTitles(): string[] {
    return this._taskItems$.value
      .map((t) => t.title?.trim())
      .filter((t): t is string => !!t);
  }

  /** Clear the currently selected task. */
  clearSelectedTask(): void {
    this._selectedTaskItem$.next(null);
  }

  /**
   * Create a new task item.
   * @param task The task item data to create.
   * @returns An observable of the created TaskItemResponseDto.
   */
  createTask(task: CreateTaskItemCommandDto): Observable<TaskItemResponseDto> {
    return this.taskItemService.create(task).pipe(
      tap((created) => {
        const current = this._taskItems$.value;
        this._taskItems$.next([...current, created]);
      })
    );
  }

  /**
   * Delete a task item.
   * @param id The ID of the task to delete.
   */
  deleteTask(id: number): void {
    this._isLoading$.next(true);
    this.taskItemService
      .delete(id)
      .pipe(finalize(() => this._isLoading$.next(false)))
      .subscribe({
        next: () => {
          const current = this._taskItems$.value.filter((t) => t.id !== id);
          this._taskItems$.next(current);
          const selected = this._selectedTaskItem$.value;
          if (selected && selected.id === id) {
            this._selectedTaskItem$.next(null);
          }
        },
        error: (err) => console.error('Failed to delete task', err),
      });
  }

  /** Load all task items from the API. */
  loadAll(): void {
    this._isLoading$.next(true);
    this.taskItemService.getAll().subscribe({
      next: (items) => {
        this._taskItems$.next(items);
        this._isLoading$.next(false);
      },
      error: (err) => {
        console.error('Failed to load tasks', err);
        this._isLoading$.next(false);
      },
    });
  }

  /**
   * Select a task by its ID. If it's not in memory, fetch from API.
   * @param id The ID of the task to select.
   */
  selectTaskById(id: number): void {
    this._isLoading$.next(true);
    const existing = this._taskItems$.value.find((t) => t.id === id);
    if (existing) {
      this._selectedTaskItem$.next(existing);
      this._isLoading$.next(false);
    } else {
      this.taskItemService.get(id).subscribe({
        next: (task) => {
          this._selectedTaskItem$.next(task);
          this._isLoading$.next(false);
        },
        error: (err) => {
          console.error('Failed to fetch task by id', err);
          this._selectedTaskItem$.next(null);
          this._isLoading$.next(false);
        },
      });
    }
  }

  /**
   * Update the status of an existing task item.
   * @param id The ID of the task to update.
   * @param status The status to update.
   */
  updateTaskStatus(id: number, status: TaskItemStatus): void {
    this._isUpdatingStatus$.next(true);
    const request: UpdateTaskItemStatusCommandDto = {
      status: status,
    };

    this.taskItemService
      .updateStatus(id, request)
      .pipe(finalize(() => this._isUpdatingStatus$.next(false)))
      .subscribe({
        next: (updated) => {
          const current = this._taskItems$.value.map((t) =>
            t.id === updated.id ? updated : t
          );
          this._taskItems$.next(current);
        },
        error: (err) => console.error('Failed to update task', err),
      });
  }
}
