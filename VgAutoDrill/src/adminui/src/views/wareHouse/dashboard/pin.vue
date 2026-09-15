<template>
  <my-template :deviceKinds="[4]" :formName="'pin'"/>
 </template>
 
 <script>
 import MyTemplate from "../components/template.vue";
 export default {
   components: { MyTemplate },
 };
 </script>
 