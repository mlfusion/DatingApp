import { AdminGuard } from './_guards/admin.guard';
import { RoleListComponent } from './roles/role-list/role-list.component';
import { RoleListResolver } from './_resolver/role-list-resolvers';

import { UserService } from './_services/user.service';
import { AuthGuard } from './_guards/auth.guard';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { NotificationService } from './_services/notification.service';
import { AuthService } from './_services/auth.service';
import { environment } from 'src/environments/environment';

import { MemberListResolver } from './_resolver/member-list-resolvers';
import { MemberDetailResolver } from './_resolver/member-detail-resolvers';
import { MemberEditResolver } from './_resolver/member-edit-resolvers';
import { PhotoListResolver } from './_resolver/photo-list-resolvers';
import { SettingsUsersResolver } from './_resolver/settings-users-resolvers';

import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {BsDropdownModule, BsDatepickerModule, TabsModule, ModalModule, ButtonsModule, PaginationModule} from 'ngx-bootstrap';
import { RouterModule } from '@angular/router';
import { appRoutes } from './routes';
import { JwtModule } from '@auth0/angular-jwt';
import { NgxGalleryModule } from 'ngx-gallery';
import { FileUploadModule } from 'ng2-file-upload';
import {TimeAgoPipe} from 'time-ago-pipe';

import { DataTablesModule } from 'angular-datatables';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ValueComponent } from './value/value.component';
import { NavComponent } from './nav/nav.component';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ValueModelComponent } from './value/valueModel/valueModel.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { PhotoListComponent } from './photos/photo-list/photo-list.component';
import { PhotoEditorComponent } from './members/photo-editor/photo-editor.component';
import { PhotoEditComponent } from './photos/photo-edit/photo-edit.component';
import { SettingsComponent } from './settings/settings/settings.component';
import { SettingsNavComponent } from './settings/settings-nav/settings-nav.component';
import { SettingsUsersComponent } from './settings/settings-users/settings-users.component';
import { Ng2SearchPipeModule } from 'ng2-search-filter';
import { SettingsUserViewComponent } from './settings/settings-user-view/settings-user-view.component';
import { RoleService } from './_services/role.service';
import { RoleEditComponent } from './roles/role-edit/role-edit.component';

// Set Auth0 Jwtmodule
export function tokenGetters() {
   return localStorage.getItem('token');
}

@NgModule({
   declarations: [
      AppComponent,
      ValueComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      ValueModelComponent,
      MemberListComponent,
      ListsComponent,
      MessagesComponent,
      MemberCardComponent,
      MemberDetailComponent,
      MemberEditComponent,
      PhotoEditorComponent,
      PhotoListComponent,
      PhotoEditComponent,
      TimeAgoPipe,
      SettingsComponent,
      SettingsNavComponent,
      SettingsUsersComponent,
      SettingsUserViewComponent,
      RoleListComponent,
      RoleEditComponent
   ],
   imports: [
      FormsModule,
      BrowserModule,
      AppRoutingModule,
      HttpClientModule,
      FormsModule,
      ReactiveFormsModule,
      NgxGalleryModule,
      FileUploadModule,
      TabsModule.forRoot(),
      BsDropdownModule.forRoot(),
      BsDatepickerModule.forRoot(),
      ButtonsModule.forRoot(),
      ModalModule.forRoot(),
      PaginationModule.forRoot(),
      RouterModule.forRoot(appRoutes),
      JwtModule.forRoot({
         config: {
            tokenGetter: tokenGetters,
            whitelistedDomains: [environment.url.replace('http://', '')],
            blacklistedRoutes: [environment.apiUrl.replace('http://', '') + 'auth']
         }
      }),
      Ng2SearchPipeModule,
      DataTablesModule
   ],
   providers: [
      AuthService,
      NotificationService,
      ErrorInterceptorProvider,
      AuthGuard,
      AdminGuard,
      PreventUnsavedChanges,
      UserService,
      MemberDetailResolver,
      MemberListResolver,
      MemberEditResolver,
      PhotoListResolver,
      SettingsUsersResolver,
      RoleListResolver,
      RoleService
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
