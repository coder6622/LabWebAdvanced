import React, { useEffect } from 'react';

function About() {
  useEffect(() => {
    document.title = 'About';
  }, []);
  return <div>About</div>;
}

export default About;
