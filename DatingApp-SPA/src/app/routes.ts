import { RoleListResolver } from './_resolver/role-list-resolvers';
import { SettingsUsersResolver } from './_resolver/settings-users-resolvers';
import { SettingsUsersComponent } from './settings/settings-users/settings-users.component';
import { SettingsComponent } from './settings/settings/settings.component';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { MemberEditResolver } from './_resolver/member-edit-resolvers';
import { MemberListResolver } from './_resolver/member-list-resolvers';
import { MemberDetailResolver } from './_resolver/member-detail-resolvers';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MessagesComponent } from './messages/messages.component';
import { HomeComponent } from './home/home.component';
import {Routes} from '@angular/router';
import { MemberListComponent } from './members/member-list/member-list.component';
import { ListsComponent } from './lists/lists.component';
import { AuthGuard } from './_guards/auth.guard';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { PhotoListComponent } from './photos/photo-list/photo-list.component';
import { PhotoListResolver } from './_resolver/photo-list-resolvers';
import { RoleListComponent } from './roles/role-list/role-list.component';
import { AdminGuard } from './_guards/admin.guard';

export const appRoutes: Routes = [
    { path: '', component: HomeComponent},
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'members', component: MemberListComponent,
                resolve: {users: MemberListResolver}},
            { path: 'members/:id', component: MemberDetailComponent,
                resolve: {user: MemberDetailResolver}},
            { path: 'member/edit', component: MemberEditComponent,
                resolve: {user: MemberEditResolver}, canDeactivate: [PreventUnsavedChanges]},
            { path: 'messages', component: MessagesComponent},
            { path: 'lists', component: ListsComponent},
            { path: 'settings', canActivate: [AdminGuard], component: SettingsComponent,
                children: [
                    { path: 'photos', component: PhotoListComponent, resolve: {photos: PhotoListResolver}},
                    { path: 'users', component: SettingsUsersComponent, resolve: {users: SettingsUsersResolver}},
                    { path: 'roles', component: RoleListComponent, resolve: {roles: RoleListResolver}},
                ]
        }
        ]
    },

    { path: '**', redirectTo: '', pathMatch: 'full'},
];
