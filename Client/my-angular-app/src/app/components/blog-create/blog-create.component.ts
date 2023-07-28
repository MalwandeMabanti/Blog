import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { BlogService } from '../../services/blog/blog.service';

@Component({
  selector: 'app-blog-create',
  templateUrl: './blog-create.component.html',
  styleUrls: ['./blog-create.component.css'],
})

export class BlogCreateComponent {
  blogForm = new FormGroup({
    title: new FormControl(''),
    content: new FormControl(''),
    image: new FormControl(''),
  });

  constructor(private blogService: BlogService) { }

  ngOnInit(): void { }

  createBlog(): void {
    const formValue = this.blogForm.value;

    if (formValue.title && formValue.content && formValue.image) {
      this.blogService.createBlog(formValue).subscribe(
        (response) => console.log('Blog created', response),
        (error) => console.log('Error creating blog', error)
      );
    } else {
      console.log('Error: All form fields must be filled out.');
    }
  }

  onFileSelected(event: Event) {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (file) {
      var reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => {
        // include the file data as part of the form data
        if (typeof reader.result === 'string') {
          this.blogForm.patchValue({
            image: reader.result
          });
        }
      };
    }
  }

}


