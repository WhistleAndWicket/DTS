import { TaskItemStatus } from '../services/dts-api';

/** Represents the data returned from the CreateTaskDialog. */
export interface CreateTaskDialogData {
  /** The description of the TaskItem. */
  description?: string | undefined;
  /** The due date of the TaskItem. */
  dueDate: Date;
  /** The status of the TaskItem. */
  status: TaskItemStatus;
  /** The title of the TaskItem. */
  title: string;
}
