import { Routes } from '@angular/router';

import { HomePageComponent } from './components/home-page/home-page.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { TaskItemComponent } from './components/task-item/task-item.component';

export const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' }, // default route
  { path: 'dashboard', component: HomePageComponent },
  { path: 'task-item/:id', component: TaskItemComponent },
  { path: '**', component: PageNotFoundComponent },
];
