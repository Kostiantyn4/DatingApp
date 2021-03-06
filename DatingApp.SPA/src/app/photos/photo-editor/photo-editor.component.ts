import { Component, OnInit, Input } from '@angular/core';
import { Photo } from '../../models/Photo';
import { FileUploader } from 'ng2-file-upload';
import { environment } from '../../../environments/environment';
import { AuthService } from '../../sevices/auth.service';
import { UserService } from '../../sevices/user.service';
import { AlertifyService } from '../../sevices/alertify.service';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {

  @Input() photos: Photo[];
  uploader: FileUploader;
  mainPhoto: Photo;
  hasBaseDropZoneOver = false;
  baseUrl = environment.baseUrl;

  constructor(private authService: AuthService,
    private userService: UserService,
    private alertify: AlertifyService) { }

  ngOnInit() {
    this.initializeUploader();
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: `${this.baseUrl}users/${this.authService.decodedToken.nameid}/photos`,
      authToken: `Bearer ${localStorage.getItem('token')}`,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      maxFileSize: 10 * 1024 * 1024 // 10 MB
    });

    // telling that the file is not going with credentials
    this.uploader.onAfterAddingFile = (file) => { file.withCredentials = false; };

    // create a photo object after it has uploaded
    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res: Photo = JSON.parse(response);
        const photo = {
          id: res.id,
          url: res.url,
          dateAdded: res.dateAdded,
          description: res.description,
          isMain: res.isMain
        };

        this.photos.push(photo);
      }
    };
  }

  setMainPhoto(photo: Photo) {
    const userId = this.authService.decodedToken.nameid;
    this.userService
      .setMainPhoto(userId, photo.id)
      .subscribe(() => {
        const previousMain = this.photos.filter(p => p.isMain === true)[0];
        previousMain.isMain = false;
        photo.isMain = true;
        this.alertify.success('Photo was set as main');
      }, error => {
        this.alertify.error(error);
      });
  }
}
