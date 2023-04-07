import React, { useEffect } from 'react';

function Contact() {
  useEffect(() => {
    document.title = 'Liên hệ';
  }, []);
  return <div>Contact</div>;
}

export default Contact;
